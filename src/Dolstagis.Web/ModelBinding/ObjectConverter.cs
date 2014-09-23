using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.ModelBinding
{
    public class ObjectConverter : IConverter
    {
        private Func<IConverter[]> _converters;

        public ObjectConverter(Func<IConverter[]> converters)
        {
            _converters = () => converters().OrderBy(x => x.Priority).ToArray();
        }

        public bool CanConvert(Type type)
        {
            return true;
        }

        public object Convert(Type type, string name, IDictionary<string, string[]> data)
        {
            var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            var converters = _converters();

            object obj = null;

            foreach (var property in properties)
            {
                object arg = null;
                var converter = converters.FirstOrDefault(x => x.CanConvert(property.PropertyType));
                if (converter != null && !(converter is ObjectConverter))
                {
                    arg = converter.Convert(property.PropertyType, property.Name, data);
                    if (arg != null)
                    {
                        obj = obj ?? Activator.CreateInstance(type);
                        property.SetValue(obj, arg);
                    }
                }
            }

            return obj;
        }


        public int Priority
        {
            get { return Int32.MaxValue; }
        }
    }
}
