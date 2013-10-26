using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web;
using Dolstagis.Web.Routing;

namespace Dolstagis.Tests.Web.Routing
{
    internal static class RouteTableExtensions
    {
        public static RouteInfo Lookup(this RouteTable routeTable, string route)
        {
            return routeTable.Lookup(new VirtualPath(route)).FirstOrDefault();
        }
    }
}
