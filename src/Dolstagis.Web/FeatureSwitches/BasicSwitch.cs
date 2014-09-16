using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.FeatureSwitches
{
    public class BasicSwitch : IFeatureSwitch
    {
        private bool _enabled;

        public Task<bool> IsEnabledForRequest(Http.IRequest request)
        {
            return Task.FromResult(_enabled);
        }

        public BasicSwitch(bool enabled)
        {
            _enabled = enabled;
        }
    }
}
