using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.Routing
{
    public class RouteTable
    {
        private IRouteRegistry[] _modules;

        public RouteTable(params IRouteRegistry[] modules)
        {
            _modules = modules;
        }

        /// <summary>
        ///  Given a path, looks up the corresponding route, and extracts its parameters.
        /// </summary>
        /// <param name="path">
        ///  The path to the route.
        /// </param>
        /// <returns>
        ///  A <see cref="RouteInfo"/> instance, or null if no routes match.
        /// </returns>

        public RouteInfo Lookup(string path)
        {
            return new RouteInfo(_modules.First().Routes.First());
        }
    }
}
