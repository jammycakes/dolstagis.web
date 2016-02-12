using System;

namespace Dolstagis.Web
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class RouteAttribute : Attribute
    {
        public VirtualPath Route { get; private set; }

        public RouteAttribute(string route)
        {
            this.Route = route;
        }
    }
}
