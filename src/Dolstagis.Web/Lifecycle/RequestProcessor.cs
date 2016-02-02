using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Dolstagis.Web.Auth;
using Dolstagis.Web.Features.Impl;
using Dolstagis.Web.Http;
using Dolstagis.Web.Lifecycle.ResultProcessors;
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
        private IAuthenticator _authenticator;
        private FeatureSet _features;
        private IIoCContainer _requestContainer;

        public RequestProcessor(
            IEnumerable<IResultProcessor> resultProcessors,
            IEnumerable<IExceptionHandler> exceptionHandlers,
            ISessionStore sessionStore,
            IAuthenticator authenticator,
            FeatureSet features,
            IIoCContainer requestContainer
        )
        {
            _resultProcessors = (resultProcessors ?? Enumerable.Empty<IResultProcessor>()).ToList();
            _exceptionHandlers = exceptionHandlers;
            _sessionStore = sessionStore;
            _authenticator = authenticator;
            _features = features;
            _requestContainer = requestContainer;
        }


        public async Task ProcessRequest(RequestContext context)
        {
            object result;
            IResultProcessor processor;

            try
            {
                result = await context.InvokeRequest();
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
            var action = new ActionInvocation();
            action.ControllerType = route.Target.ControllerType;
            var method = action.ControllerType.GetMethod(request.Method,
                BindingFlags.Instance | BindingFlags.IgnoreCase |
                BindingFlags.Public | BindingFlags.DeclaredOnly);
            if (method == null)
            {
                if (request.Method.Equals("head", StringComparison.OrdinalIgnoreCase))
                {
                    method = action.ControllerType.GetMethod("get",
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
            return new RequestContext(request, response, _sessionStore, _authenticator, _requestContainer) {
                Actions = GetActions(request).ToList()
            };
        }
    }
}
