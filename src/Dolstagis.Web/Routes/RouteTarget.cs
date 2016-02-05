using System;
using System.Linq.Expressions;

namespace Dolstagis.Web.Routes
{
    public class RouteTarget : IRouteTarget
    {
        private Expression<Func<IServiceProvider, object>> _controllerExpr;
        private Func<IServiceProvider, object> _controllerFunc;

        private Expression<Func<IServiceProvider, IModelBinder>> _modelBinderExpr;

        public object GetController(IServiceProvider provider)
        {
            return _controllerFunc(provider);
        }

        public Type ControllerType { get; set; }

        public RouteTarget(Type controllerType)
        {
            ControllerType = controllerType;

            var method = typeof(IServiceProvider).GetMethod("GetService", new Type[] { typeof(Type) });
            var serviceProviderParam = Expression.Parameter(typeof(IServiceProvider), "provider");
            var typeParam = Expression.Constant(controllerType, typeof(Type));
            var resultParam = Expression.Parameter(typeof(object), "result");
            var methodCallExpr = Expression.Call(serviceProviderParam, method, typeParam);

            _controllerExpr =
                Expression.Lambda<Func<IServiceProvider, object>>(methodCallExpr, serviceProviderParam);
            _controllerFunc = _controllerExpr.Compile();
        }

        public RouteTarget(Expression<Func<IServiceProvider, object>> controllerExpr)
        {
            _controllerExpr = controllerExpr;
            _controllerFunc = _controllerExpr.Compile();

            ControllerType = ((_controllerExpr.Body as MethodCallExpression)?.Arguments?[0]
                as ConstantExpression)?.Value as Type;
        }
    }
}
