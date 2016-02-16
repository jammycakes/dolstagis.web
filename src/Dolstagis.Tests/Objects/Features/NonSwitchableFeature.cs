using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web;
using Dolstagis.Web.IoC.DSL;

namespace Dolstagis.Tests.Objects.Features
{
    public class NonSwitchableFeature : Feature
    {
        public NonSwitchableFeature(Action<IContainerExpression> configureMe)
        {
            Description("NonSwitchable");
            configureMe(Container);
        }
    }
}
