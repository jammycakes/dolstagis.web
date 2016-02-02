using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.Views.DotLiquid
{
    public class DotLiquidFeature : Feature
    {
        public DotLiquidFeature()
        {
            Container.Setup.Application(c => {
                c.Add<IViewEngine, DotLiquidViewEngine>(Scope.Application);
            });
        }
    }
}
