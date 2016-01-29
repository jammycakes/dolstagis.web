using Dolstagis.Web.Http;
using Dolstagis.Web.Routes;
using Dolstagis.Web.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Dolstagis.Web.ModelBinding
{
    public class ModelBinder : IModelBinder
    {
        private static readonly IConverter[] defaultConverters = new IConverter[] {
            new BoolConverter(),
            new DateTimeConverter(),
            new GuidConverter(),
            new IntConverter(),
            new LongConverter(),
            new StringConverter(),
            new ObjectConverter(() => defaultConverters)
        };

        public static ModelBinder Default { get; private set; }

        static ModelBinder()
        {
            Default = new ModelBinder();
        }

        public IConverter[] Converters { get; set; }

        private StringComparer ParameterComparer { get; set; }

        public ModelBinder()
        {
            Converters = defaultConverters;
            ParameterComparer = StringComparer.OrdinalIgnoreCase;
        }

        public object[] GetArguments(RouteInvocation route, IRequest request, MethodInfo method)
        {
            var foundArgs = new Dictionary<string, string[]>(ParameterComparer);
            foundArgs.Concat(route.RouteData);
            foundArgs.Concat(request.Query);
            foundArgs.Concat(request.Form);

            var args = new List<object>();
            foreach (var parameter in method.GetParameters())
            {
                object arg = null;
                var converter = Converters.FirstOrDefault
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
                        "Required argument {0} was not supplied to method {1} on controller {2}",
                        parameter.Name, method.Name, method.DeclaringType));
                }
            }

            return args.ToArray();
        }
    }
}
