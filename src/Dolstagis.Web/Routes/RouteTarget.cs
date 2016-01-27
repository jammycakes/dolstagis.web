using System;

namespace Dolstagis.Web.Routes
{
    public class RouteTarget : IRouteTarget
    {

        public Type HandlerType { get; set; }

        public RouteTarget(Type handlerType)
        {
            this.HandlerType = handlerType;
        }
    }
}
