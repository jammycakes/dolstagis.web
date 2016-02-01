using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dolstagis.Web.Auth;
using Dolstagis.Web.Http;
using Dolstagis.Web.Sessions;

namespace Dolstagis.Web.Lifecycle
{
    public class RequestContext
    {
        private ISessionStore _sessionStore;
        private IAuthenticator _authenticator;
        private ISession _session = null;
        private IUser _user = null;

        public RequestContext(IRequest request, IResponse response,
            ISessionStore sessionStore, IAuthenticator authenticator)
        {
            Request = request;
            Response = response;
            _sessionStore = sessionStore;
            _authenticator = authenticator;
        }


        public IRequest Request { get; private set; }

        public IResponse Response { get; private set; }

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

        public IList<ActionInvocation> Actions { get; set; }


        /* ====== Invoking the request ====== */

        public async Task<object> InvokeRequest()
        {
            var actions = Actions.Where(x => x.Method != null);

            foreach (var action in actions) {
                if (IsLoginRequired(action)) {
                    return await GetLoginResult();
                }
                var result = action.Invoke(this);
                if (result is Task) {
                    await (Task)result;
                    return ((dynamic)result).Result;
                }
                else if (result != null) {
                    return result;
                }
            }
            throw new HttpStatusException(Status.NotFound);
        }


        protected virtual bool IsLoginRequired(ActionInvocation action)
        {
            var attributes = action.Method.GetCustomAttributes(true).OfType<IRequirement>()
                .Concat(action.ControllerType.GetCustomAttributes(true).OfType<IRequirement>());
            return attributes.Any(x => x.IsDenied(this));
        }

        protected virtual Task<object> GetLoginResult()
        {
            var result = new RedirectResult("/login", Status.SeeOther);
            return Task.FromResult<object>(result);
        }
    }
}
