using Dolstagis.Web.Http;
using Dolstagis.Web.Routes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web
{
    /// <summary>
    ///  Binds the data from an <see cref="IRequest"/> instance to a 
    /// </summary>

    public interface IModelBinder
    {
        object[] GetArguments(RouteInvocation route, IRequest request, MethodInfo method);
    }
}
