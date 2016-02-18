using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Features;

namespace Dolstagis.Web.Routes
{
    public class RouteTable : Trie.Trie<RouteNode, IRouteTarget>, IRouteTable
    {
        public RouteInvocation GetRouteInvocation(VirtualPath path, IFeature feature)
        {
            var match = this.GetMatches(path).FirstOrDefault();
            if (match == null) return null;
            var routeTarget = match.Node.Items.FirstOrDefault();
            if (routeTarget == null) return null;
            var data =
                from param in match.Parameters
                let value = param.Value.Any(x => x != null)
                    ? String.Join(",", param.Value.Where(x => x != null))
                    : null
                select new { param.Key, value };

            return new RouteInvocation(
                feature, routeTarget,
                data.ToDictionary(x => x.Key, x => x.value)
            );
        }
    }
}
