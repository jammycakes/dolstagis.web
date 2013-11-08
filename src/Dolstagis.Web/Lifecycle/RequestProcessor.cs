using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Http;
using Dolstagis.Web.Sessions;
using Dolstagis.Web.Static;

namespace Dolstagis.Web.Lifecycle
{
    public class RequestProcessor : IRequestProcessor
    {
        private IList<IResultProcessor> _resultProcessors;
        private IExceptionHandler _exceptionHandler;
        private IHttpContextBuilder _contextBuilder;

        public RequestProcessor(
            IEnumerable<IResultProcessor> resultProcessors,
            IExceptionHandler exceptionHandler,
            IHttpContextBuilder contextBuilder
        )
        {
            _resultProcessors = (resultProcessors ?? Enumerable.Empty<IResultProcessor>()).ToList();
            _exceptionHandler = exceptionHandler;
            _contextBuilder = contextBuilder;
        }


        public async Task<object> InvokeRequest(IHttpContext context)
        {
            if (context == null) Status.NotFound.Throw();

            var actions = context.Actions.Where(x => x.Method != null);
            foreach (var action in actions)
            {
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

        public async Task<object> InvokeRequestWithHomePageFallback(IHttpContext context)
        {
            if (context.Request.AppRelativePath.Parts.Any()
                || context.Actions.Any())
            {
                return await InvokeRequest(context);
            }
            else
            {
                return new StaticResult(new VirtualPath("~/_dolstagis/index.html"));
            }
        }

        public async Task ProcessRequest(IHttpContext context)
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
                    context.Response.AddCookie(cookie);
                }
            }
            await processor.Process(result, context);
        }

        public async Task ProcessRequest(RequestContext request, ResponseContext response)
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
                await _exceptionHandler.HandleException(context, fault);
            }

            if (context.Session != null) await context.Session.Persist();
        }
    }
}
