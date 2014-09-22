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
        public bool CanConvert(Type type)
        {
            throw new NotImplementedException();
        }

        public object Convert(Type type, string name, IDictionary<string, string[]> data)
        {
            throw new NotImplementedException();
        }
    }
}
