using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dolstagis.Web.Auth;
using Dolstagis.Web.Features.Impl;
using Dolstagis.Web.Http;
using Dolstagis.Web.IoC;
using Dolstagis.Web.Lifecycle.ResultProcessors;
using Dolstagis.Web.Sessions;

namespace Dolstagis.Web.Lifecycle
{
    public class RequestProcessor
    {
        private ISettings _settings;
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
            IIoCContainer featureSetContainer,
            ISettings settings
        )
        {
            _exceptionHandlers = exceptionHandlers;
            _sessionStore = sessionStore;
            _authenticator = authenticator;
            _features = features;
            _featureSetContainer = featureSetContainer;
            _settings = settings;
        }

        public async Task ProcessRequestAsync(IRequest request, IResponse response)
        {
            using (var childContainer = _featureSetContainer.GetChildContainer()) {
                var context = new RequestContext
                    (request, response, _sessionStore, _authenticator, childContainer, _features);
                childContainer.Add(Binding<IRequestContext>
                    .From(cfg => cfg.Only().To(context).Managed()));

                foreach (var feature in _features.Features) {
                    feature.ContainerBuilder.SetupRequest(childContainer);
                }

                if (_settings.Debug) {
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
                }

                await ProcessRequestContextAsync(context);
            }
        }


        private async Task ProcessRequestContextAsync(RequestContext context)
        {
            try {
                object result;

                try {
                    result = await context.InvokeRequest();
                }
                finally {
                    await context.PersistSession();
                }

                if (!await ProcessResultAsync(context, result)) Status.NotAcceptable.Throw();
            }
            catch (Exception ex) {
                await HandleException(context, ex);
            }
        }

        private async Task<bool> ProcessResultAsync(IRequestContext context, object result)
        {
            var resultObj = result as IResult;
            if (resultObj == null) return false;
            await resultObj.RenderAsync(context);
            return true;
        }


        private async Task HandleException(IRequestContext context, Exception ex)
        {
            var fault = ex;
            while (fault is AggregateException && ((AggregateException)fault).InnerExceptions.Count == 1) {
                fault = ((AggregateException)fault).InnerExceptions.Single();
            }
            foreach (var handler in _exceptionHandlers)
            {
                await handler.HandleException(context, fault);
            }
            //await ProcessResultAsync(context, fault);
        }
        
        // TODO: this is only used in one of the tests. Need to either refactor the test
        // or else use [InternalsVisibleTo]. We shouldn't be exposing RequestContext
        // in the public API, only IRequestContext.

        public RequestContext CreateContext(IRequest request, IResponse response, IIoCContainer requestContainer)
        {
            return new RequestContext(request, response, _sessionStore, _authenticator, requestContainer, _features);
        }
    }
}
