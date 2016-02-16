using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web;
using Dolstagis.Web.IoC;
using Dolstagis.Web.StructureMap;

namespace Dolstagis.Tests.Objects.Features
{
    public class ContainerFeature<TContainer> : Feature
        where TContainer : class, IIoCContainer, new()
    {
        public ContainerFeature
            (When whenToAddSwitch = When.Neither, TContainer instance = null)
        {
            if (whenToAddSwitch.HasFlag(When.Before)) {
                Active.When(() => false);
            }
            var containerConfig = Container.Is<TContainer>();
            if (instance != null) containerConfig.Using(instance);

            if (whenToAddSwitch.HasFlag(When.After)) {
                Active.When(() => false);
            }
        }
    }
}
