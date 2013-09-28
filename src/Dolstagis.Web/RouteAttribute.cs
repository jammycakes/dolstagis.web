using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class RouteAttribute : Attribute
    {
        public string Route { get; private set; }

        public RouteAttribute(string route)
        {
            this.Route = route;
        }
    }
}
