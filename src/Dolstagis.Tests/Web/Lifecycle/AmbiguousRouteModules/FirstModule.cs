using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web;

namespace Dolstagis.Tests.Web.Lifecycle.AmbiguousRouteModules
{
    public class FirstModule : Module
    {
        public FirstModule()
        {
            this.AddHandler<FirstHandler>("HandledByFirst");
            this.AddHandler<NullHandler>("HandledBySecond");
        }
    }
}
