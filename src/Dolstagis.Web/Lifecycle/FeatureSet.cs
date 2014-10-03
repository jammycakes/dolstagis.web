using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Http;
using Dolstagis.Web.Routes;
using StructureMap;

namespace Dolstagis.Web.Lifecycle
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
        /// <summary>
        ///  Gets the Application object.
        /// </summary>

        public Application Application { get; private set; }

        /// <summary>
        ///  Gets the IOC container scoped to this feature set.
        /// </summary>

        public IContainer Container { get; private set; }

        /// <summary>
        ///  Gets the features in this feature set.
        /// </summary>

        public IReadOnlyCollection<Feature> Features { get; private set; }

        public FeatureSet(Application application, IEnumerable<Feature> features)
        {
            this.Application = application;
            this.Features = features.ToList().AsReadOnly();
            if (application != null)
            {
                this.Container = application.Container.GetNestedContainer();
                this.Container.Configure(x =>
                {
                    x.For<FeatureSet>().Use(this);
                    foreach (var feature in this.Features)
                    {
                        x.AddRegistry(feature.Services);
                        x.For<Feature>().Add(feature);
                    }
                });
            }
        }

        public async Task ProcessRequestAsync(IRequest request, IResponse response)
        {
            using (var childContainer = Container.GetNestedContainer())
            {
                childContainer.Configure(x =>
                {
                    x.For<IRequest>().Use(request);
                    x.For<IResponse>().Use(response);
                });
                await childContainer.GetInstance<RequestProcessor>()
                    .ProcessRequest(request, response);
            }
        }


        public RouteInvocation GetRouteInvocation(IRequest request)
        {
            var result =
                from feature in Features
                let invocation = feature.Routes.GetRouteInvocation(request.Path)
                where invocation != null && invocation.Target != null
                select invocation;
            return result.LastOrDefault();
        }
    }
}
