﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Http;
using Dolstagis.Web.Routing;

namespace Dolstagis.Web.Lifecycle
{
    public class RequestProcessor : IRequestProcessor
    {
        private RouteTable _routes;
        private Func<ActionInvocation> _createAction;

        public RequestProcessor(RouteTable routes, Func<ActionInvocation> createAction)
        {
            _routes = routes;
            _createAction = createAction;
        }

        public ActionInvocation GetAction(IHttpContext context)
        {
            var route = _routes.Lookup(context.Request.AppRelativePath);
            if (route == null) return null;
            var action = _createAction();
            action.HandlerType = route.Definition.HandlerType;
            var method = action.HandlerType.GetMethod(context.Request.Method,
                BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.DeclaredOnly);
            if (method == null) return action;
            action.Method = method;
            var args = new List<object>();
            foreach (var parameter in method.GetParameters()) {
                string arg;
                if (route.Arguments.TryGetValue(parameter.Name, out arg)) {
                    args.Add(arg);
                }
                else if (parameter.IsOptional) {
                    args.Add(parameter.HasDefaultValue ? parameter.DefaultValue : null);
                }
                else {
                    throw new InvalidOperationException(String.Format(
                        "Required argument {0} was not supplied to method {1} on handler {2}",
                        parameter.Name, method.Name, action.HandlerType));
                }
            }
            action.Parameters = args.ToArray();
            return action;
        }

        public async Task<object> ProcessRequest(IHttpContext context)
        {
            var action = GetAction(context);
            if (action == null) Status.NotFound.Throw();
            if (action.Method == null) Status.MethodNotAllowed.Throw();
            var result = action.Invoke();
            if (result == null) Status.NotFound.Throw();
            if (result is Task) {
                await (Task)result;
                return ((dynamic)result).Result;
            }
            else {
                return result;
            }
        }
    }
}