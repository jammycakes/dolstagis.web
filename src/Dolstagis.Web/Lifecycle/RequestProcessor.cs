using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using Dolstagis.Web.Auth;
using Dolstagis.Web.ContentNegotiation;
using Dolstagis.Web.Features.Impl;
using Dolstagis.Web.Http;
using Dolstagis.Web.IoC;
using Dolstagis.Web.Sessions;

namespace Dolstagis.Web.Lifecycle
{
    public class RequestProcessor
    {
        private ISettings _settings;
        private Interceptors _interceptors;
        private ISessionStore _sessionStore;
        private IAuthenticator _authenticator;
        private FeatureSet _features;
        private IIoCContainer _featureSetContainer;
        private IArbitrator _negotiator;

        private bool _requestContainerIsChecked = false;
        private bool _requestContainerIsValid = false;

        public RequestProcessor(
            IEnumerable<IInterceptor> interceptors,
            ISessionStore sessionStore,
            IAuthenticator authenticator,
            FeatureSet features,
            IIoCContainer featureSetContainer,
            ISettings settings,
            IArbitrator negotiator
        )
        {
            _interceptors = new Interceptors(interceptors);
            _sessionStore = sessionStore;
            _authenticator = authenticator;
            _features = features;
            _featureSetContainer = featureSetContainer;
            _settings = settings;
            _negotiator = negotiator;
        }

        public async Task ProcessRequestAsync(IRequest request, IResponse response)
        {
            using (var childContainer = _featureSetContainer.GetChildContainer()) {
                IRequestContext context = new RequestContext
                    (request, response, _sessionStore, _authenticator, childContainer, _features);
                context = await BuildRequestContextAsync(childContainer, context);
                context = await _interceptors.BeginRequest(context);
                childContainer.Add(Binding<IRequestContext>
                    .From(cfg => cfg.Only().To(context).Managed()));
                await ProcessRequestContextAsync(context);
            }
        }

        private async Task<IRequestContext> BuildRequestContextAsync
            (IIoCContainer childContainer, IRequestContext context)
        {
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

            return context;
        }

        private async Task ProcessRequestContextAsync(IRequestContext context)
        {
            var outputContext =
                context.Request.Method.Equals("HEAD", StringComparison.OrdinalIgnoreCase)
                ? new BodylessRequestContextDecorator(context)
                : (IRequestContext)context;

            try {
                try {
                    object result = await context.InvokeRequest();
                    if (!await ProcessResultAsync(outputContext, result))
                        throw Status.NotAcceptable.CreateException();
                }
                catch (Exception ex) {
                    ex = await _interceptors.HandleException(context, ex);
                    ExceptionDispatchInfo.Capture(ex).Throw();
                }
            }
            catch (HttpStatusException hex) {
                await ProcessResultAsync(outputContext, new StatusResult(hex.Status));
            }
            catch (Exception ex) {
                await ProcessResultAsync(outputContext, new ExceptionResult(ex));
            }
        }

        private async Task<bool> ProcessResultAsync(IRequestContext context, object result)
        {
            IResult resultObj = result is Status
                ? new StatusResult((Status)result)
                : result as IResult;
            resultObj = resultObj ?? _negotiator.Arbitrate(context.Request, result);
            if (resultObj == null) return false;
            await resultObj.RenderAsync(context);
            return true;
        }


        // TODO: this is only used in one of the tests. Need to either refactor the test
        // or else use [InternalsVisibleTo]. We shouldn't be exposing RequestContext
        // in the public API, only IRequestContext.

        public RequestContext CreateContext(IRequest request, IResponse response, IIoCContainer requestContainer)
        {
            return new RequestContext(request, response, _sessionStore, _authenticator,
                requestContainer, _features, _interceptors);
        }
    }
}
