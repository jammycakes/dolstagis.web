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

namespace Dolstagis.Web.Lifecycle
{
    public class RequestProcessor
    {
        private IList<IResultProcessor> _resultProcessors;
        private IEnumerable<IExceptionHandler> _exceptionHandlers;
        private ISessionStore _sessionStore;
        private IAuthenticator _authenticator;
        private FeatureSet _features;
        private IIoCContainer _featureSetContainer;

        private bool _requestContainerIsChecked = false;
        private bool _requestContainerIsValid = false;

        public RequestProcessor(
            IEnumerable<IResultProcessor> resultProcessors,
            IEnumerable<IExceptionHandler> exceptionHandlers,
            ISessionStore sessionStore,
            IAuthenticator authenticator,
            FeatureSet features,
            IIoCContainer featureSetContainer
        )
        {
            _resultProcessors = (resultProcessors ?? Enumerable.Empty<IResultProcessor>()).ToList();
            _exceptionHandlers = exceptionHandlers;
            _sessionStore = sessionStore;
            _authenticator = authenticator;
            _features = features;
            _featureSetContainer = featureSetContainer;
        }

        public async Task ProcessRequestAsync(IRequest request, IResponse response)
        {
            using (var childContainer = _featureSetContainer.GetChildContainer()) {
                childContainer.Use<IRequest>(request);
                childContainer.Use<IResponse>(response);

                foreach (var feature in _features.Features) {
                    feature.ContainerBuilder.SetupRequest(childContainer);
                }

                lock (_featureSetContainer) {
                    // Only validate the request container once.
                    // But throw every time if it fails.
                    if (_requestContainerIsChecked) {
                        if (!_requestContainerIsValid) {
                            throw new InvalidOperationException(
                                "The container configuration for requests for this feature set is invalid. "
                                + "Please refer to previous errors."
                            );
                        }
                    }
                    else {
                        try {
                            childContainer.Validate();
                            _requestContainerIsValid = true;
                        }
                        finally {
                            _requestContainerIsChecked = true;
                        }
                    }
                }

                await ProcessRequestContextAsync(context);
            }
        }


        public async Task ProcessRequestContextAsync(RequestContext context)
        {
            try {
                object result;
                IResultProcessor processor;

                try {
                    result = await context.InvokeRequest();
                }
                finally {
                    await context.PersistSession();
                }

                if (context.Request.Method.Equals("head", StringComparison.OrdinalIgnoreCase)) {
                    processor = HeadResultProcessor.Instance;
                }
                else {
                    processor = _resultProcessors.LastOrDefault(x => x.CanProcess(result));
                }
                if (processor == null) Status.NotFound.Throw();
                await processor.Process(result, context);
            }
            catch (Exception ex) {
                var fault = ex;
                while (fault is AggregateException && ((AggregateException)fault).InnerExceptions.Count == 1) {
                    fault = ((AggregateException)fault).InnerExceptions.Single();
                }
                await HandleException(context, fault);
            }
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

        public RequestContext CreateContext(IRequest request, IResponse response, IIoCContainer requestContainer)
        {
            return new RequestContext(request, response, _sessionStore, _authenticator, requestContainer) {
                Actions = GetActions(request).ToList()
            };
        }
    }
}
