using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web;
using Dolstagis.Web.IoC.DSL;

namespace Dolstagis.Tests.Objects.Features
{
    public class SwitchableFeature : Feature
    {
        public SwitchableFeature(bool isOn, Action<IContainerExpression> configureMe = null)
        {
            Description("Switchable:" + isOn.ToString());
            Active.When(() => isOn);
            if (configureMe != null) configureMe(Container);
        }
    }
}
