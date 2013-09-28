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
            this.Arguments = new Dictionary<string, string>();
        }

        public IRouteDefinition Definition { get; private set; }

        public IDictionary<string, string> Arguments { get; private set; }
    }
}
