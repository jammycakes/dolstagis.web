using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Routes;

namespace Dolstagis.Web.Features.Impl
{
    public class RouteExpression : IRouteExpression
    {
        private RouteTable _routes;

        public RouteExpression(RouteTable routes)
        {
            _routes = routes;
        }

        public IRouteFromExpression From(VirtualPath path)
        {
            return new RouteFromExpression(_routes, path);
        }
    }
}
