using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Http;

namespace Dolstagis.Web.Routes
{
    public interface IRouteTable
    {
        /// <summary>
        ///  Registers a route on this route table.
        /// </summary>
        /// <param name="specification">
        ///  The route specification. null or an empty string indicates the root
        ///  node.
        /// </param>
        /// <param name="target">
        ///  The route target.
        /// </param>
        /// <param name="constraint">
        ///  A predicate which allows us to add constraints to the route.
        /// </param>

        void Add(string specification, IRouteTarget target, Predicate<IRequest> constraint);

        /// <summary>
        ///  Gets the route target for the requested path.
        /// </summary>
        /// <param name="path">
        ///  The path of the request.
        /// </param>
        /// <returns>
        ///  The route target.
        /// </returns>

        RouteInvocation GetRouteInvocation(VirtualPath path);
    }
}
