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


        public Application Application { get; private set; }

        public FeatureSwitchboard(Application application)
        {
            this.Application = application;
        }

        public FeatureSwitchboard Add(params IFeatureSwitch[] switches)
        {
            this.switches.AddRange(switches);
            this.featureSets.Clear();
            return this;
        }


        public FeatureSwitchboard Add(params Feature[] features)
        {
            this.switches.AddRange(features.Select(f => new AlwaysEnabledFeatureSwitch(f)));
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

            return new FeatureSet(Application, features);
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

        private class AlwaysEnabledFeatureSwitch : IFeatureSwitch
        {
            public AlwaysEnabledFeatureSwitch(Feature feature)
            {
                this.Feature = feature;
            }

            public async Task<bool> IsEnabledForRequest(IRequest request)
            {
                return true;
            }

            public Feature Feature { get; private set; }

            public bool DependentOnRequest { get { return false; } }
        }
    }
}
