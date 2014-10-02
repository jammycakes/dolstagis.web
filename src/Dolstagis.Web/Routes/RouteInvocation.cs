using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.Routes
{
    public class RouteInvocation
    {
        public Feature Feature { get; private set; }

        public IRouteTarget Target { get; private set; }

        public IDictionary<string, string> RouteData { get; private set; }

        public RouteInvocation
            (Feature feature, IRouteTarget target, IDictionary<string, string> data)
        {
            this.Feature = feature;
            this.Target = target;
            this.RouteData = data;
        }
    }
}
