using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web;
using NUnit.Framework;

namespace Dolstagis.Tests.Web.Lifecycle.AmbiguousRouteModules
{
    public class SecondHandler : Handler
    {
        public object Get()
        {
            return "Second";
        }
    }
}
