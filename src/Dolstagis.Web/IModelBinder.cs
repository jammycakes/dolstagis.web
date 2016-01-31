using System.Reflection;
using Dolstagis.Web.Http;
using Dolstagis.Web.Routes;

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
