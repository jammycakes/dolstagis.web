using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web;

namespace Dolstagis.Tests.Web.Lifecycle.AmbiguousRouteModules
{
    public class SecondModule : Module
    {
        public SecondModule()
        {
            this.AddHandler<NullHandler>("HandledByFirst");
            this.AddHandler<SecondHandler>("HandledBySecond");
        }
    }
}
