using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.ModelBinding
{
    public abstract class SimpleConverter<T> : IConverter
    {
        protected abstract object Parse(string s);

        public bool CanConvert(Type type)
        {
            return typeof(T).IsAssignableFrom(type);
        }


        public object Convert(Type type, string name, IDictionary<string, string[]> data)
        {
            string[] values;
            if (!data.TryGetValue(name, out values)) return null;
            return Parse(values.Last());
        }
    }
}
