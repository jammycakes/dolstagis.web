using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.Static
{
    public class StaticRequestHandler : Handler
    {
        public object Get(string path = "")
        {
            return new StaticResult(Context.Request.Path);
        }
    }
}
