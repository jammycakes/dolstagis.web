using System;

namespace Dolstagis.Web.Routes
{
    public interface IRouteTarget
    {
        Type HandlerType { get; }
    }
}
