using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Auth;
using Dolstagis.Web.Http;
using Dolstagis.Web.Routes;
using Dolstagis.Web.Sessions;
using Dolstagis.Web.Static;

namespace Dolstagis.Web.Lifecycle
{
    public class RequestProcessor
    {
        private IList<IResultProcessor> _resultProcessors;
        private IEnumerable<IExceptionHandler> _exceptionHandlers;
        private ISessionStore _sessionStore;
        private Func<ActionInvocation> _createAction;
        private IAuthenticator _authenticator;
        private FeatureSet _features;

        public RequestProcessor(
            IEnumerable<IResultProcessor> resultProcessors,
            IEnumerable<IExceptionHandler> exceptionHandlers,
            ISessionStore sessionStore,
            IAuthenticator authenticator,
            FeatureSet features,
            Func<ActionInvocation> createAction
        )
        {
            _resultProcessors = (resultProcessors ?? Enumerable.Empty<IResultProcessor>()).ToList();
            _exceptionHandlers = exceptionHandlers;
            _sessionStore = sessionStore;
            _createAction = createAction;
            _authenticator = authenticator;
            _features = features;
        }


        protected virtual bool IsLoginRequired(RequestContext context, ActionInvocation action)
        {
            var attributes = action.Method.GetCustomAttributes(true).OfType<IRequirement>()
                .Concat(action.HandlerType.GetCustomAttributes(true).OfType<IRequirement>());
            return attributes.Any(x => x.IsDenied(context));
        }

        protected virtual Task<object> GetLoginResult(RequestContext context)
        {
            var result = new RedirectResult("/login", Status.SeeOther);
            return Task.FromResult<object>(result);
        }


        public async Task<object> InvokeRequest(RequestContext context)
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

        public async Task<object> InvokeRequestWithHomePageFallback(RequestContext context)
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

        public async Task ProcessRequest(RequestContext context)
        {
            object result;
            IResultProcessor processor;

            try
            {
                result = await InvokeRequestWithHomePageFallback(context);
                if (context.Request.Method.Equals("head", StringComparison.OrdinalIgnoreCase))
                {
                    processor = HeadResultProcessor.Instance;
                }
                else
                {
                    processor = _resultProcessors.LastOrDefault(x => x.CanProcess(result));
                }
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
            var context = CreateContext(request, response);
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

        public virtual async Task HandleException(RequestContext context, Exception fault)
        {
            foreach (var handler in _exceptionHandlers)
            {
                await handler.HandleException(context, fault);
            }
        }

        private IEnumerable<ActionInvocation> GetActions(IRequest request)
        {
            var routeInvocation = _features.GetRouteInvocation(request);
            if (routeInvocation != null) {
                var action = GetAction(request, routeInvocation);
                yield return action;
            }
        }

        private ActionInvocation GetAction(IRequest request, RouteInvocation route)
        {
            var action = _createAction();
            action.HandlerType = route.Target.HandlerType;
            var method = action.HandlerType.GetMethod(request.Method,
                BindingFlags.Instance | BindingFlags.IgnoreCase |
                BindingFlags.Public | BindingFlags.DeclaredOnly);
            if (method == null)
            {
                if (request.Method.Equals("head", StringComparison.OrdinalIgnoreCase))
                {
                    method = action.HandlerType.GetMethod("get",
                        BindingFlags.Instance | BindingFlags.IgnoreCase |
                        BindingFlags.Public | BindingFlags.DeclaredOnly);
                    if (method == null)
                    {
                        return action;
                    }
                }
                else
                {
                    return action;
                }
            }
                
            action.Method = method;
            action.Arguments = route.Feature.ModelBinder.GetArguments(route, request, method);
            return action;
        }

        public RequestContext CreateContext(IRequest request, IResponse response)
        {
            var actions = GetActions(request);
            var session = GetSession(request);
            return new RequestContext(request, response, session, _authenticator, actions);
        }

        private ISession GetSession(IRequest request)
        {
            if (_sessionStore == null) return null;

            Cookie sessionCookie;
            string sessionID =
                request.Headers.Cookies.TryGetValue(Constants.SessionKey, out sessionCookie)
                ? sessionCookie.Value
                : null;

            return _sessionStore.GetSession(sessionID);
        }
    }
}
