using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Tests.Web.TestModules.Handlers;
using Dolstagis.Web;

namespace Dolstagis.Tests.Web.TestModules
{
    public class DisabledModule : Module
    {
        public DisabledModule()
        {
            this.Enabled = false;
            this.AddHandler<ChildHandler>("/foo/bar");
        }
    }
}
