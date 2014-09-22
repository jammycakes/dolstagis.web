using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.ModelBinding
{
    public interface IConverter
    {
        bool CanConvert(Type type);

        object Convert(Type type, string name, IDictionary<string, string[]> data);
    }
}
