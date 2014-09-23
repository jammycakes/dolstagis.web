using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.ModelBinding
{
    public class StringConverter : SimpleConverter<string>
    {
        protected override object Parse(string s)
        {
            return s;
        }
    }
}
