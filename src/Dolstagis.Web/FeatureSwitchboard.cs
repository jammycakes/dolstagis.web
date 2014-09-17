using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.FeatureSwitches;
using Dolstagis.Web.Http;

namespace Dolstagis.Web
{
    /// <summary>
    ///  Provides a mechanism to fetch the appropriate feature set for a request.
    /// </summary>

    public class FeatureSwitchboard
    {
        private List<FeatureSwitchLink> switches = new List<FeatureSwitchLink>();

        private Dictionary<ulong, FeatureSet> featureSets = new Dictionary<ulong, FeatureSet>();

        public Application Application { get; private set; }

        public FeatureSwitchboard(Application application)
        {
            this.Application = application;
        }


        private IFeatureSwitch GetFeatureSwitch(Feature feature)
        {
            var switches =
                from attr in feature.GetType().GetCustomAttributes(true)
                let factory = attr as IFeatureSwitchBuilder
                let @switch = factory != null
                    ? factory.CreateSwitch(feature, this.Application)
                    : attr as IFeatureSwitch
                where @switch != null
                select @switch;
            return switches.FirstOrDefault() ?? new BasicSwitch(true);
        }


        public FeatureSwitchboard Add(params Feature[] features)
        {
            var links =
                from feature in features
                let @switch = GetFeatureSwitch(feature)
                select new FeatureSwitchLink(@switch, feature);

            this.switches.AddRange(links);
            return this;
        }


        public FeatureSwitchboard Add(IFeatureSwitch @switch, Feature feature)
        {
            this.switches.Add(new FeatureSwitchLink(@switch, feature));
            return this;
        }


        private async Task<ulong> GetKey(IRequest request)
        {
            ulong key = 0;
            foreach (var sw in switches) {
                checked {
                    key = key << 1;
                }
                if (await sw.Switch.IsEnabledForRequest(request)) {
                    key++;
                }
            }
            return key;
        }

        private async Task<FeatureSet> CreateFeatureSet(IRequest request)
        {
            var features = new List<Feature>();
            foreach (var sw in switches) {
                if (await sw.Switch.IsEnabledForRequest(request)) {
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


        private class FeatureSwitchLink
        {
            public IFeatureSwitch Switch { get; private set; }

            public Feature Feature { get; private set; }

            public FeatureSwitchLink(IFeatureSwitch @switch, Feature feature)
            {
                this.Switch = @switch;
                this.Feature = feature;
            }
        }
    }
}
