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
                    (request, response, _sessionStore, _authenticator, childContainer, _features, _interceptors);
                try {
                    BuildRequestContext(childContainer, context);
                    context = await _interceptors.BeginRequest(context);
                    childContainer.Add(Binding<IRequestContext>
                        .From(cfg => cfg.Only().To(context).Managed()));
                    ValidateContainer(childContainer);
                    await ProcessRequestContextAsync(context);
                }
                finally {
                    await _interceptors.EndRequest(context);
                }
            }
        }

        private void BuildRequestContext(IIoCContainer childContainer, IRequestContext context)
        {
            foreach (var feature in _features.Features) {
                feature.ContainerBuilder.SetupRequest(childContainer);
            }
        }

        private void ValidateContainer(IIoCContainer container)
        {
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
                            container.Validate();
                            _requestContainerIsValid = true;
                        }
                        finally {
                            _requestContainerIsChecked = true;
                        }
                    }
                }
            }
        }

        private async Task ProcessRequestContextAsync(IRequestContext context)
        {
            if (context.Request.Method.Equals("HEAD", StringComparison.OrdinalIgnoreCase))
                context = new BodylessRequestContextDecorator(context);

            try {
                object result = await context.InvokeRequest();
                IResult resultObj = GetResultObject(context.Request, result);
                resultObj = await _interceptors.NegotiatedResult(context, resultObj);
                if (resultObj == null)
                    throw Status.NotAcceptable.CreateException();
                await resultObj.RenderAsync(context);
            }
            catch (Exception ex) {
                await HandleException(context, ex);
            }
        }


        private IResult GetResultObject(IRequest request, object result)
        {
            if (result is IResult)
                return (IResult)result;
            if (result == null)
                return new StatusResult(Status.NotFound);
            if (result is Status)
                return new StatusResult((Status)result);
            return _negotiator.Arbitrate(request, result);
        }


        private async Task HandleException(IRequestContext context, Exception ex)
        {
            try {
                ex = await _interceptors.Exception(context, ex, false);
                var result = ex is HttpStatusException
                    ? new StatusResult(((HttpStatusException)ex).Status)
                    : new ExceptionResult(ex);
                await result.RenderAsync(context);
            }
            catch (Exception ex1) {
                ex1 = await _interceptors.Exception(context, ex1, true);
                ExceptionDispatchInfo.Capture(ex1).Throw();
            }
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
