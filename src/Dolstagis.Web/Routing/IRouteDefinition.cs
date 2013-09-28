using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.Routing
{
    /// <summary>
    ///  Represents an entry in the route table.
    /// </summary>

    public interface IRouteDefinition
    {
        /// <summary>
        ///  Gets the type of handler which will be instantiated by this route.
        /// </summary>

        Type HandlerType { get; }

        /// <summary>
        ///  Verifies that the preconditions for this route have been met.
        /// </summary>
        /// <param name="routeInfo"></param>
        /// <returns></returns>

        bool IsPreconditionMet(RouteInfo routeInfo);

        /// <summary>
        ///  Gets the module in which this route was declared.
        /// </summary>

        IRouteRegistry Module { get; }

        /// <summary>
        ///  Gets the string representation of the route.
        /// </summary>

        string Route { get; }
    }
}
