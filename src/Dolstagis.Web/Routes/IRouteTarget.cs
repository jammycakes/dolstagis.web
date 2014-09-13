using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.Routes
{
    public interface IRouteTarget
    {
        Type HandlerType { get; }

        MethodInfo Method { get; }
    }
}
