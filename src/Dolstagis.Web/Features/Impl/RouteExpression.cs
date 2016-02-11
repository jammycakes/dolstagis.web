using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Routes;
using Dolstagis.Web.Util;

namespace Dolstagis.Web.Features.Impl
{
    public class RouteExpression : IRouteExpression
    {
        private RouteTable _routes;
        private Feature _feature;

        public RouteExpression(RouteTable routes, Feature feature)
        {
            _routes = routes;
            _feature = feature;
        }

        public IRouteFromExpression From(VirtualPath path)
        {
            return new RouteFromExpression(_routes, path);
        }


        public void Controller(Type controllerType)
        {
            var routeAttribute = (RouteAttribute)
                controllerType.GetCustomAttributes(typeof(RouteAttribute), true)
                .FirstOrDefault();
            if (routeAttribute == null) {
                throw new ArgumentException(
                    "The controller of type " + controllerType.FullName +
                    " is not marked with a RouteAttribute. You will need to either " +
                    "add a [Route(path)] attribute or else specify the route " +
                    "explicitly in the feature definition.");
            }
            From(routeAttribute.Route).To.Controller(services => services.Get(controllerType));
        }


        public void Controller<TController>()
        {
            Controller(typeof(TController));
        }


        public void AllControllersInAssembly()
        {
            var types =
                from type in _feature.GetType().Assembly.SafeGetTypes<object>()
                where
                    !type.IsAbstract &&
                    !type.IsInterface &&
                    !type.IsGenericType &&
                    type.GetCustomAttributes(typeof(RouteAttribute), true).Any()
                select type;
            foreach (var type in types)
                Controller(type);
        }
    }
}
