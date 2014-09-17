using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Auth;
using Dolstagis.Web.Http;
using Dolstagis.Web.Routes;
using Dolstagis.Web.Sessions;

namespace Dolstagis.Web.Lifecycle
{
    public class RequestContextBuilder : IRequestContextBuilder
    {
        private ISessionStore _sessionStore;
        private Func<ActionInvocation> _createAction;
        private IAuthenticator _authenticator;
        private FeatureSet _features;

        public RequestContextBuilder(ISessionStore sessionStore,
            IAuthenticator authenticator,
            FeatureSet features,
            Func<ActionInvocation> createAction)
        {
            _sessionStore = sessionStore;
            _createAction = createAction;
            _authenticator = authenticator;
            _features = features;
        }

        public IEnumerable<ActionInvocation> GetActions(IRequest request)
        {
            var routeInvocation = _features.GetRouteInvocation(request);
            if (routeInvocation != null) {
                var action = GetAction(request, routeInvocation);
                yield return action;
            }
        }

        private ActionInvocation GetAction(IRequest request, RouteInvocation route)
        {
            var action = _createAction();
            action.HandlerType = route.Target.HandlerType;
            var method = action.HandlerType.GetMethod(request.Method,
                BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.DeclaredOnly);
            if (method == null) return action;
            action.Method = method;
            var args = new List<object>();
            foreach (var parameter in method.GetParameters())
            {
                object arg;
                if (route.RouteData.TryGetValue(parameter.Name, out arg))
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

        public IRequestContext CreateContext(IRequest request, IResponse response)
        {
            var actions = GetActions(request);
            var session = GetSession(request);
            return new RequestContext(request, response, session, _authenticator, actions);
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
