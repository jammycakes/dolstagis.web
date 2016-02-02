using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dolstagis.Web.Http;
using Dolstagis.Web.Lifecycle;
using Dolstagis.Web.Routes;

namespace Dolstagis.Web.Features.Impl
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

        public IIoCContainer Container { get; private set; }

        /// <summary>
        ///  Gets the features in this feature set.
        /// </summary>

        public IReadOnlyCollection<IFeature> Features { get; private set; }

        public FeatureSet(Application application, IEnumerable<IFeature> features)
        {
            this.Application = application;
            this.Features = features.ToList().AsReadOnly();
            if (application != null)
            {
                this.Container = application.Container.GetChildContainer();
                this.Container.Use<FeatureSet>(this);
                this.Container.Use<RequestProcessor, RequestProcessor>(Scope.Application);
                foreach (var feature in Features) {
                    Container.Add<IFeature>(feature);
                    feature.ContainerBuilder.SetupDomain(this.Container);
                }
                this.Container.Validate();
            }
        }

        public RequestProcessor GetRequestProcessor()
        {
            return this.Container.GetService<RequestProcessor>();
        }


        public RouteInvocation GetRouteInvocation(IRequest request)
        {
            var result =
                from feature in Features
                let invocation = feature.GetRouteInvocation(request.Path)
                where invocation != null && invocation.Target != null
                select invocation;
            return result.LastOrDefault();
        }
    }
}
