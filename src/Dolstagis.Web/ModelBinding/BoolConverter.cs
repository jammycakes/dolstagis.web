using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Util;

namespace Dolstagis.Web.ModelBinding
{
    public class BoolConverter : SimpleConverter<bool>
    {
        protected override object Parse(string s)
        {
            return bool.Parse(s);
        }
    }
}
