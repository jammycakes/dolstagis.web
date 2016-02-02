using System.Collections.Generic;
using Dolstagis.Web.Features;

namespace Dolstagis.Web.Routes
{
    public class RouteInvocation
    {
        public IFeature Feature { get; private set; }

        public IRouteTarget Target { get; private set; }

        public IDictionary<string, string> RouteData { get; private set; }

        public RouteInvocation
            (IFeature feature, IRouteTarget target, IDictionary<string, string> data)
        {
            this.Feature = feature;
            this.Target = target;
            this.RouteData = data;
        }
    }
}
