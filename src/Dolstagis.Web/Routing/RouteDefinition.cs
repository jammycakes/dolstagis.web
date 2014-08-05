using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.Routing
{
    public class RouteDefinition : IRouteDefinition
    {
        private Func<RouteInfo, bool> _precondition;

        public RouteDefinition(Type handlerType, string route, IRouteRegistry feature, Func<RouteInfo, bool> precondition)
        {
            this.HandlerType = handlerType;
            this.Route = route;
            this.Feature = feature;
            this._precondition = precondition;
        }

        public Type HandlerType { get; private set; }

        public string Route { get; private set; }

        public bool IsPreconditionMet(RouteInfo routeInfo)
        {
            return _precondition(routeInfo);
        }

        public IRouteRegistry Feature { get; private set; }
    }
}
