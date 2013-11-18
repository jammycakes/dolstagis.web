using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Dolstagis.Web.Auth;
using Dolstagis.Web.Http;
using Dolstagis.Web.Lifecycle;
using Dolstagis.Web.Sessions;

namespace Dolstagis.Web
{
    public class HttpContext : IHttpContext
    {
        private IAuthenticator _authenticator;
        private IUser _user;

        public HttpContext(IRequestContext request, IResponseContext response,
            ISession session,
            IAuthenticator authenticator,
            IEnumerable<ActionInvocation> actions)
        {
            Request = request;
            Response = response;
            Session = session;
            _authenticator = authenticator;
            Actions = actions.ToList();
        }

        public IRequestContext Request { get; private set; }

        public IResponseContext Response { get; private set; }

        public ISession Session { get; private set; }

        private bool _fetchingUser;

        public IUser User
        {
            get
            {
                if (_fetchingUser) return null;
                if (_user == null && _authenticator != null)
                {
                    try
                    {
                        _fetchingUser = true;
                        _user = _authenticator.GetUser(this);
                    }
                    finally
                    {
                        _fetchingUser = false;
                    }
                }
                return _user;
            }
            set
            {
                _authenticator.SetUser(this, value);
                _user = value;
            }
        }

        public IList<ActionInvocation> Actions { get; private set; }

    }
}
