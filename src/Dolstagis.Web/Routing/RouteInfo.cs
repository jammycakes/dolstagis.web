using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.Routing
{
    public class RouteInfo
    {
        public RouteInfo(IRouteDefinition definition)
        {
            this.Definition = definition;
        }

        public IRouteDefinition Definition { get; private set; }
    }
}
