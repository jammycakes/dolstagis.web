using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dolstagis.Web.Http;
using Dolstagis.Web.Logging;

namespace Dolstagis.Web.Features.Impl
{
    /// <summary>
    ///  Provides a mechanism to fetch the appropriate feature set for a request.
    /// </summary>

    public class FeatureSwitchboard
    {
        private static readonly Logger log = Logger.ForThisClass();

        private List<Feature> _features = new List<Feature>();
        private Dictionary<Key, FeatureSet> featureSets = new Dictionary<Key, FeatureSet>();

        public Application Application { get; private set; }


        public FeatureSwitchboard(Application application)
        {
            this.Application = application;
        }


        private IFeatureSwitch GetFeatureSwitch(IFeature feature)
        {
            return feature.Switch;
        }


        public FeatureSwitchboard Add(params Feature[] features)
        {
            this._features.AddRange(features);
            return this;
        }


        private Key GetKey(IRequest request)
        {
            var state = new List<bool>();
            foreach (var sw in _features) {
                state.Add(sw.Switch.IsEnabledForRequest(request));
            }

            return new Key(state);
        }

        private FeatureSet CreateFeatureSet(IRequest request)
        {
            var features = new List<Feature>();
            foreach (var sw in _features) {
                if (sw.Switch.IsEnabledForRequest(request)) {
                    log.Trace(() => "Feature " + sw.GetType().FullName + " enabled - adding");
                    features.Add(sw);
                }
                else {
                    log.Trace(() => "Feature " + sw.GetType().FullName + " disabled");
                }
            }

            return new FeatureSet(Application, features);
        }

        public FeatureSet GetFeatureSet(IRequest request)
        {
            var key = GetKey(request);
            FeatureSet result;
            if (!featureSets.TryGetValue(key, out result)) {
                log.Debug(() => "Requesting feature set with key: " + key.ToString());
                result = CreateFeatureSet(request);
                featureSets.Add(key, result);
            }
            else
            {
                log.Debug(() => "Using existing feature set with key: " + key.ToString());
            }
            return result;
        }


        internal class Key
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

            public override string ToString()
            {
                return String.Concat(this.state.Select(x => x.ToString("X8")));
            }
        }
    }
}
