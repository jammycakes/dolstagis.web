using System;
using System.Collections.Generic;
using System.Linq;

namespace Dolstagis.Web.ModelBinding
{
    public abstract class SimpleConverter<T> : IConverter
    {
        protected abstract object Parse(string s);

        public bool CanConvert(Type type)
        {
            return typeof(T).IsAssignableFrom(type) || typeof(T[]).IsAssignableFrom(type);
        }


        public object Convert(Type type, string name, IDictionary<string, string[]> data)
        {
            string[] values;
            if (!data.TryGetValue(name, out values)) return null;
            return type.IsArray ? values.Select(Parse).OfType<T>().ToArray() : Parse(values.Last());
        }


        public int Priority
        {
            get { return 0; }
        }
    }
}
