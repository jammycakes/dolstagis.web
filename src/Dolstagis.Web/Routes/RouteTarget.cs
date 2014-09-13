using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.Routes
{
    public class RouteTarget : IRouteTarget
    {

        public Type HandlerType { get; set; }

        public RouteTarget(Type handlerType)
        {
            this.HandlerType = handlerType;
        }
    }
}
