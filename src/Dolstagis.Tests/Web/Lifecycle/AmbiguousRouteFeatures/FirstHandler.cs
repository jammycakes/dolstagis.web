using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web;

namespace Dolstagis.Tests.Web.Lifecycle.AmbiguousRouteFeatures
{
    public class FirstHandler : Handler
    {
        public object Get()
        {
            return "First";
        }
    }
}
