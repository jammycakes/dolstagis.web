using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Http;
using Dolstagis.Web.Routing;
using Dolstagis.Web.Views;

namespace Dolstagis.Web.Lifecycle
{
    public class RequestProcessor : IRequestProcessor
    {
        private RouteTable _routes;
        private IList<IResultProcessor> _resultProcessors;
        private Func<ActionInvocation> _createAction;

        public RequestProcessor(RouteTable routes,
            IEnumerable<IResultProcessor> resultProcessors,
            Func<ActionInvocation> createAction)
        {
            _routes = routes;
            _resultProcessors = (resultProcessors ?? Enumerable.Empty<IResultProcessor>()).ToList();
            _createAction = createAction;
        }

        public ActionInvocation GetAction(IRequestContext context)
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

        public async Task<object> InvokeRequest(IRequestContext context)
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

        public async Task ProcessRequest(IRequestContext context)
        {
            var result = await InvokeRequest(context);
            var processor = _resultProcessors.LastOrDefault(x => x.CanProcess(result));
            if (processor == null) Status.NotFound.Throw();
            await processor.Process(result, context);
        }
    }
}
