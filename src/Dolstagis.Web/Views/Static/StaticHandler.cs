using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.Views.Static
{
    public class StaticHandler : Handler
    {
        public object Get(string path)
        {
            return new StaticResult(path);
        }
    }
}
