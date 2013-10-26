using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web;

namespace Dolstagis.Tests.Web.Lifecycle.AmbiguousRouteModules
{
    public class ArgHandler : Handler
    {
        public object Get(string arg)
        {
            return arg;
        }
    }
}
