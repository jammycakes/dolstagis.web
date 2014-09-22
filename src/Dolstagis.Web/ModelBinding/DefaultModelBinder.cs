using Dolstagis.Web.Http;
using Dolstagis.Web.Routes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.ModelBinding
{
    public class DefaultModelBinder : IModelBinder
    {
        public object[] GetArguments(RouteInvocation route, IRequest request, MethodInfo method)
        {
            var args = new List<object>();
            foreach (var parameter in method.GetParameters())
            {
                object arg;
                if (route.RouteData.TryGetValue(parameter.Name, out arg))
                {
                    args.Add(arg);
                }
                else if (parameter.IsOptional)
                {
                    args.Add(parameter.HasDefaultValue ? parameter.DefaultValue : null);
                }
                else
                {
                    throw new InvalidOperationException(String.Format(
                        "Required argument {0} was not supplied to method {1} on handler {2}",
                        parameter.Name, method.Name, method.DeclaringType));
                }
            }

            return args.ToArray();
        }
    }
}
