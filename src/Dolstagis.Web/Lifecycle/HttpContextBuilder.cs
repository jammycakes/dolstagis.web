using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Auth;
using Dolstagis.Web.Http;
using Dolstagis.Web.Routing;
using Dolstagis.Web.Sessions;

namespace Dolstagis.Web.Lifecycle
{
    public class HttpContextBuilder : IHttpContextBuilder
    {
        private RouteTable _routes;
        private ISessionStore _sessionStore;
        private Func<ActionInvocation> _createAction;
        private IAuthenticator _authenticator;

        public HttpContextBuilder(RouteTable routes, ISessionStore sessionStore,
            IAuthenticator authenticator,
            Func<ActionInvocation> createAction)
        {
            _routes = routes;
            _sessionStore = sessionStore;
            _createAction = createAction;
            _authenticator = authenticator;
        }

        public IEnumerable<ActionInvocation> GetActions(IRequest request)
        {
            return _routes.Lookup(request.Path)
                .Select(route => GetAction(request, route));
        }

        private ActionInvocation GetAction(IRequest request, RouteInfo route)
        {
            var action = _createAction();
            action.HandlerType = route.Definition.HandlerType;
            var method = action.HandlerType.GetMethod(request.Method,
                BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.DeclaredOnly);
            if (method == null) return action;
            action.Method = method;
            var args = new List<object>();
            foreach (var parameter in method.GetParameters())
            {
                string arg;
                if (route.Arguments.TryGetValue(parameter.Name, out arg))
                {
                    args.Add(arg);
                }
                else if (parameter.IsOptional)
                {
                    args.Add(parameter.HasDefaultValue ? parameter.DefaultValue : null);
                }
                else
                {
                    throw new InvalidOperationException(String.Format(
                        "Required argument {0} was not supplied to method {1} on handler {2}",
                        parameter.Name, method.Name, action.HandlerType));
                }
            }
            action.Parameters = args.ToArray();
            return action;
        }

        public IHttpContext CreateContext(IRequest request, IResponseContext response)
        {
            var actions = GetActions(request);
            var session = GetSession(request);
            return new HttpContext(request, response, session, _authenticator, actions);
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
