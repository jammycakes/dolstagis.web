using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Http;

namespace Dolstagis.Web
{
    /// <summary>
    ///  A container for the group of features which are enabled for a
    ///  particular request. A feature set also acts as the base handler class
    ///  for a particular group of requests, providing us with a chain of
    ///  responsibility for all the different services such as dependency
    ///  injection/service location, routing, content negotiation, view and
    ///  static file handling, and so on, which vary solely depending
    ///  on which features are turned on or off.
    /// </summary>

    public class FeatureSet
    {
        public IReadOnlyCollection<Feature> Features { get; private set; }

        public FeatureSet(IEnumerable<Feature> features)
        {
            this.Features = features.ToList().AsReadOnly();
        }

        public async Task ProcessRequestAsync(IRequest request, IResponse response)
        {
            await Task.Yield();
        }
    }
}
