using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.Routes
{
    public class RouteInvocation
    {
        public IRouteTarget Target { get; private set; }

        public IDictionary<string, object> RouteData { get; private set; }

        public RouteInvocation(IRouteTarget target, IDictionary<string, object> data)
        {
            this.Target = target;
            this.RouteData = data;
        }
    }
}
