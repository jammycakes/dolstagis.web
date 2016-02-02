using System;

namespace Dolstagis.Web.Routes
{
    public class RouteTarget : IRouteTarget
    {
        public Type ControllerType { get; set; }

        public RouteTarget(Type controllerType)
        {
            ControllerType = controllerType;
        }
    }
}
