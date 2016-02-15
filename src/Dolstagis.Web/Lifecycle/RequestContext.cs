using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Dolstagis.Web.Auth;
using Dolstagis.Web.Features.Impl;
using Dolstagis.Web.IoC;
using Dolstagis.Web.Http;
using Dolstagis.Web.Sessions;
using System.Collections;
using System.Runtime.ExceptionServices;

namespace Dolstagis.Web.Lifecycle
{
    public class RequestContext : IRequestContext
    {
        private ISessionStore _sessionStore;
        private IAuthenticator _authenticator;
        private ISession _session = null;
        private IUser _user = null;
        private IServiceLocator _container;

        public RequestContext(IRequest request, IResponse response,
            ISessionStore sessionStore, IAuthenticator authenticator,
            IIoCContainer container, IFeatureSet features)
        {
            Request = request;
            Response = response;
            _sessionStore = sessionStore;
            _authenticator = authenticator;
            _container = new ContainerWrapper(container);
            Features = features;
        }

        public IServiceLocator Container { get { return _container; } }

        public IRequest Request { get; private set; }

        public IResponse Response { get; private set; }

        public IFeatureSet Features { get; private set; }

        public ISession Session {
            get {
                return _session = _session ?? GetSessionAsync().Result;
            }
        }

        public IUser User
        {
            get {
                return _user = _user ?? GetUserAsync().Result;
            }
        }

        public async Task<ISession> GetSessionAsync()
        {
            if (_sessionStore == null) return null;
            if (_session == null) {
                Cookie sessionCookie;
                string sessionID =
                    Request.Headers.Cookies.TryGetValue(Constants.SessionKey, out sessionCookie)
                    ? sessionCookie.Value
                    : null;
                _session = await _sessionStore.GetSession(sessionID);
            }
            return _session;
        }

        public async Task<IUser> GetUserAsync()
        {
            if (_authenticator == null) return null;
            _user = _user ?? await _authenticator.GetUser(this);
            return _user;
        }


        /* ====== Invoking the request ====== */

        public async Task<object> InvokeRequest()
        {
            try {
                return await InvokeRequestInternal();
            }
            finally {
                await PersistSession();
            }
        }

        private async Task<object> InvokeRequestInternal()
        {
            var invocation = Features.GetRouteInvocation(Request);
            var controller = invocation.Target.GetController(Container);
            if (controller == null) return Status.NotFound;
            Type controllerType = controller.GetType();

            var method = controllerType.GetMethod(Request.Method,
                BindingFlags.Instance | BindingFlags.IgnoreCase |
                BindingFlags.Public | BindingFlags.DeclaredOnly);
            if (method == null) {
                if (Request.Method.Equals("head", StringComparison.OrdinalIgnoreCase)) {
                    method = controllerType.GetMethod(Request.Method,
                        BindingFlags.Instance | BindingFlags.IgnoreCase |
                        BindingFlags.Public | BindingFlags.DeclaredOnly);
                }
            }

            if (method == null) return Status.NotFound;

            var modelBinder = invocation.Feature.ModelBinder;
            var arguments = modelBinder.GetArguments(invocation, Request, method);

            if (IsLoginRequired(method)) {
                return await GetLoginResult();
            }

            object result = null;
            try {
                result = method.Invoke(controller, arguments);
            }
            catch (TargetInvocationException tex) {
                ExceptionDispatchInfo.Capture(tex.InnerException).Throw();
            }
            if (result is Task) {
                await (Task)result;
                return ((dynamic)result).Result;
            }
            else if (result != null) {
                return result;
            }

            return Status.NotFound;
        }

        protected virtual bool IsLoginRequired(MethodInfo method)
        {
            var attributes = method.GetCustomAttributes(true).OfType<IRequirement>()
                .Concat(method.DeclaringType.GetCustomAttributes(true).OfType<IRequirement>());
            return attributes.Any(x => x.IsDenied(this));
        }

        protected virtual Task<object> GetLoginResult()
        {
            var result = new RedirectResult("/login", Status.SeeOther);
            return Task.FromResult<object>(result);
        }


        private async Task PersistSession()
        {
            if (_session != null) {
                if (_session.ID != null) {
                    var cookie = new Cookie(Constants.SessionKey, _session.ID) {
                        Expires = _session.Expires,
                        HttpOnly = true,
                        Secure = Request.IsSecure
                    };
                    Response.Headers.AddCookie(cookie);
                }
                await _session.Persist();
            }
        }


        private class ContainerWrapper : IServiceLocator
        {
            private IServiceLocator _provider;

            public ContainerWrapper(IServiceLocator provider)
            {
                _provider = provider;
            }

            public object Get(Type serviceType)
            {
                return _provider.Get(serviceType);
            }

            public IEnumerable GetAll(Type itemType)
            {
                return _provider.GetAll(itemType);
            }
        }
    }
}
