using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StructureMap;

namespace Dolstagis.Web
{
    public interface IFeatureSwitchBuilder
    {
        IFeatureSwitch CreateSwitch(Feature feature, Application application);
    }
}
