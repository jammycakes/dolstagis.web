using System;
using Dolstagis.Web.IoC;

namespace Dolstagis.Web.Routes
{
    public interface IRouteTarget
    {
        object GetController(IServiceLocator provider);

        [Obsolete("This is deprecated. Use GetController instead.", true)]
        Type ControllerType { get; }
    }
}
