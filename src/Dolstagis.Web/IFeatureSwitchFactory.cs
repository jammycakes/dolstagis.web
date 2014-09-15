using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web
{
    public interface IFeatureSwitchFactory
    {
        IFeatureSwitch CreateSwitch(Feature feature);
    }
}
