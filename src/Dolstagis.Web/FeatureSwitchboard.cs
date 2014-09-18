using System;
using System.Collections;
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

        private Dictionary<Key, FeatureSet> featureSets = new Dictionary<Key, FeatureSet>();

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


        private async Task<Key> GetKey(IRequest request)
        {
            var state = new List<bool>();
            foreach (var sw in switches) {
                state.Add(await sw.Switch.IsEnabledForRequest(request));
            }

            return new Key(state);
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
            var key = await GetKey(request);
            FeatureSet result;
            if (!featureSets.TryGetValue(key, out result)) {
                result = await CreateFeatureSet(request);
                featureSets.Add(key, result);
            }
            return result;
        }



        private class Key
        {
            // I'd much rather use BitArray for this, but it doesn't override
            // Equals() and GetHashCode() so it's no good as a key for hashtables.

            private uint[] state;

            public Key(IEnumerable<bool> featureStates)
            {
                var state = new List<uint>();
                const int chunkSize = sizeof(uint);
                uint current = 0;
                int ix = 0;
                foreach (var fs in featureStates) {
                    current <<= 1;
                    if (fs) current++;
                    if (++ix == chunkSize) {
                        state.Add(current);
                        current = 0;
                        ix = 0;
                    }
                }
                if (ix > 0) state.Add(current);
                this.state = state.ToArray();
            }


            public override bool Equals(object obj)
            {
                if (obj is Key) {
                    return StructuralComparisons.StructuralEqualityComparer.Equals
                        (this.state, ((Key)obj).state);
                }
                else {
                    return false;
                }
            }

            public override int GetHashCode()
            {
                return (int)state.FirstOrDefault();
            }
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
