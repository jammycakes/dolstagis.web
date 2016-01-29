using System;

namespace Dolstagis.Web.Routes
{
    public interface IRouteTarget
    {
        Type ControllerType { get; }
    }
}
