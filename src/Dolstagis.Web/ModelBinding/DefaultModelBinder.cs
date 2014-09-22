using Dolstagis.Web.Http;
using Dolstagis.Web.Routes;
using Dolstagis.Web.Util;
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
        private IConverter[] _converters;
        
        public DefaultModelBinder(IConverter[] converters)
        {
            _converters = converters.OrderBy(x => x.Priority).ToArray();
        }

        public object[] GetArguments(RouteInvocation route, IRequest request, MethodInfo method)
        {
            var foundArgs = new Dictionary<string, string[]>();
            foundArgs.Concat(route.RouteData);
            foundArgs.Concat(request.Query);
            foundArgs.Concat(request.Form);

            var args = new List<object>();
            foreach (var parameter in method.GetParameters())
            {
                object arg = null;
                var converter = _converters.FirstOrDefault
                    (x => x.CanConvert(parameter.ParameterType));
                if (converter != null) {
                    arg = converter.Convert(parameter.ParameterType, parameter.Name, foundArgs);
                }
                if ((converter == null || arg == null) && parameter.IsOptional)
                {
                    arg = (parameter.HasDefaultValue ? parameter.DefaultValue : null);
                }
                    

                if (arg != null)
                {
                    args.Add(arg);
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
