using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Tests.Web.TestFeatures.Handlers;
using Dolstagis.Web;

namespace Dolstagis.Tests.Web.TestFeatures
{
    public class DisabledFeature : Feature
    {
        public DisabledFeature()
        {
            this.Enabled = false;
            this.AddHandler<ChildHandler>("/foo/bar");
        }
    }
}
