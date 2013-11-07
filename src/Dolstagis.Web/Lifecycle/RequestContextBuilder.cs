using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Http;
using Dolstagis.Web.Routing;
using Dolstagis.Web.Sessions;

namespace Dolstagis.Web.Lifecycle
{
    public class RequestContextBuilder : IRequestContextBuilder
    {
        private RouteTable _routes;
        private ISessionStore _sessionStore;
        private Func<ActionInvocation> _createAction;

        public RequestContextBuilder(RouteTable routes, ISessionStore sessionStore,
            Func<ActionInvocation> createAction)
        {
            _routes = routes;
            _sessionStore = sessionStore;
            _createAction = createAction;
        }

        public IEnumerable<ActionInvocation> GetActions(IRequest request)
        {
            return _routes.Lookup(request.AppRelativePath)
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

        public IHttpContext CreateContext(RequestContext request, ResponseContext response)
        {
            var actions = GetActions(request);
            return new HttpContext(request, response, GetSession(request), actions);
        }

        private ISession GetSession(RequestContext request)
        {
            if (_sessionStore == null) return null;

            Cookie sessionCookie;
            string sessionID =
                request.Cookies.TryGetValue(Constants.SessionKey, out sessionCookie)
                ? sessionCookie.Value
                : null;

            return _sessionStore.GetSession(sessionID);
        }
    }
}
