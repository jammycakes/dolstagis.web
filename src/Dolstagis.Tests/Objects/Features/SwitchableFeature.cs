using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web;

namespace Dolstagis.Tests.Objects.Features
{
    public class SwitchableFeature : Feature
    {
        public SwitchableFeature(bool isOn)
        {
            Active.When(() => isOn);

        }
    }
}
