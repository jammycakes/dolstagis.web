using System;

namespace Dolstagis.Web.Routes
{
    public interface IRouteTarget
    {
        object GetController(IServiceProvider provider);

        Type ControllerType { get; }
    }
}
