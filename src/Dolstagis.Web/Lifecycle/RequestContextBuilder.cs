﻿using System;
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
                BindingFlags.Instance | BindingFlags.IgnoreCase |
                BindingFlags.Public | BindingFlags.DeclaredOnly);
            if (method == null)
            {
                if (request.Method.Equals("head", StringComparison.OrdinalIgnoreCase))
                {
                    method = action.HandlerType.GetMethod("get",
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
