using System;

namespace Dolstagis.Web.Routes
{
    public interface IRouteTarget
    {
        object GetController(IServiceProvider provider);

        [Obsolete("This is deprecated. Use GetController instead.", true)]
        Type ControllerType { get; }
    }
}
