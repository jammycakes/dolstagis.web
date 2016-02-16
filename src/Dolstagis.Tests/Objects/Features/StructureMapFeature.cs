using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web;
using Dolstagis.Web.StructureMap;

namespace Dolstagis.Tests.Objects.Features
{
    public class StructureMapFeature : Feature
    {
        public StructureMapFeature
            (When whenToAddSwitch = When.Neither, StructureMapContainer instance = null)
        {
            if (whenToAddSwitch.HasFlag(When.Before)) {
                Active.When(() => false);
            }
            var containerConfig = Container.Is<StructureMapContainer>();
            if (instance != null) containerConfig.Using(instance);

            if (whenToAddSwitch.HasFlag(When.After)) {
                Active.When(() => false);
            }
        }
    }
}
