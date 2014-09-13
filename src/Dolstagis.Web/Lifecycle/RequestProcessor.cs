using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Auth;
using Dolstagis.Web.Http;
using Dolstagis.Web.Sessions;
using Dolstagis.Web.Static;

namespace Dolstagis.Web.Lifecycle
{
    public class RequestProcessor : IRequestProcessor
    {
        private IList<IResultProcessor> _resultProcessors;
        private IEnumerable<IExceptionHandler> _exceptionHandlers;
        private IRequestContextBuilder _contextBuilder;

        public RequestProcessor(
            IEnumerable<IResultProcessor> resultProcessors,
            IEnumerable<IExceptionHandler> exceptionHandlers,
            IRequestContextBuilder contextBuilder
        )
        {
            _resultProcessors = (resultProcessors ?? Enumerable.Empty<IResultProcessor>()).ToList();
            _exceptionHandlers = exceptionHandlers;
            _contextBuilder = contextBuilder;
        }


        protected virtual bool IsLoginRequired(IRequestContext context, ActionInvocation action)
        {
            var attributes = action.Method.GetCustomAttributes(true).OfType<IRequirement>()
                .Concat(action.HandlerType.GetCustomAttributes(true).OfType<IRequirement>());
            return attributes.Any(x => x.IsDenied(context));
        }

        protected virtual Task<object> GetLoginResult(IRequestContext context)
        {
            var result = new RedirectResult("/login", Status.SeeOther);
            return Task.FromResult<object>(result);
        }


        public async Task<object> InvokeRequest(IRequestContext context)
        {
            if (context == null) Status.NotFound.Throw();

            var actions = context.Actions.Where(x => x.Method != null);

            foreach (var action in actions)
            {
                if (IsLoginRequired(context, action))
                {
                    return await GetLoginResult(context);
                }
                var result = action.Invoke(context);
                if (result is Task)
                {
                    await (Task)result;
                    return ((dynamic)result).Result;
                }
                else if (result != null)
                {
                    return result;
                }
            }
            throw new HttpStatusException(Status.NotFound);
        }

        public async Task<object> InvokeRequestWithHomePageFallback(IRequestContext context)
        {
            if (context.Request.Path.Parts.Any()
                || context.Actions.Any())
            {
                return await InvokeRequest(context);
            }
            else
            {
                return new StaticResult(new VirtualPath("~/_dolstagis/index.html"));
            }
        }

        public async Task ProcessRequest(IRequestContext context)
        {
            object result;
            IResultProcessor processor;

            try
            {
                result = await InvokeRequestWithHomePageFallback(context);
                processor = _resultProcessors.LastOrDefault(x => x.CanProcess(result));
                if (processor == null) Status.NotFound.Throw();
            }
            finally
            {
                if (context.Session != null && context.Session.ID != null)
                {
                    var cookie = new Cookie(Constants.SessionKey, context.Session.ID)
                    {
                        Expires = context.Session.Expires,
                        HttpOnly = true,
                        Secure = context.Request.IsSecure
                    };
                    context.Response.Headers.AddCookie(cookie);
                }
            }
            await processor.Process(result, context);
        }

        public async Task ProcessRequest(IRequest request, IResponse response)
        {
            var context = _contextBuilder.CreateContext(request, response);
            Exception fault = null;
            try
            {
                await ProcessRequest(context);
            }
            catch (Exception ex)
            {
                fault = ex;
            }

            if (fault != null)
            {
                while (fault is AggregateException && ((AggregateException)fault).InnerExceptions.Count == 1)
                {
                    fault = ((AggregateException)fault).InnerExceptions.Single();
                }
                await HandleException(context, fault);
            }

            if (context.Session != null) await context.Session.Persist();
        }

        public virtual async Task HandleException(IRequestContext context, Exception fault)
        {
            foreach (var handler in _exceptionHandlers)
            {
                await handler.HandleException(context, fault);
            }
        }
    }
}
