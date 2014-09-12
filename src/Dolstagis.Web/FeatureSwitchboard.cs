using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Http;

namespace Dolstagis.Web
{
    /// <summary>
    ///  Provides a mechanism to fetch the appropriate feature set for a request.
    /// </summary>

    public class FeatureSwitchboard
    {
        private List<IFeatureSwitch> switches = new List<IFeatureSwitch>();

        private Dictionary<ulong, FeatureSet> featureSets = new Dictionary<ulong, FeatureSet>();

        public FeatureSwitchboard(IEnumerable<IFeatureSwitch> switches)
        {
            this.switches.AddRange(switches);
        }

        public FeatureSwitchboard Add(params IFeatureSwitch[] switches)
        {
            this.switches.AddRange(switches);
            this.featureSets.Clear();
            return this;
        }

        private async Task<ulong> GetKey(IRequest request)
        {
            ulong key = 0;
            foreach (var sw in switches)
            {
                if (sw.DependentOnRequest)
                {
                    checked { 
                        key = key << 1;
                    }
                    if (await sw.IsEnabledForRequest(request))
                    {
                        key++;
                    }
                }
            }
            return key;
        }

        private async Task<FeatureSet> CreateFeatureSet(IRequest request)
        {
            var features = new List<Feature>();
            foreach (var sw in switches) {
                if (await sw.IsEnabledForRequest(request)) {
                    features.Add(sw.Feature);
                }
            }

            return new FeatureSet(features);
        }

        public async Task<FeatureSet> GetFeatureSet(IRequest request)
        {
            ulong key = await GetKey(request);
            FeatureSet result;
            if (!featureSets.TryGetValue(key, out result)) {
                result = await CreateFeatureSet(request);
                featureSets.Add(key, result);
            }
            return result;
        }
    }
}
