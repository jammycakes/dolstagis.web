using System.Collections.Generic;
using Dolstagis.Web.Http;
using Dolstagis.Web.IoC;
using Dolstagis.Web.Lifecycle;
using Dolstagis.Web.Routes;

namespace Dolstagis.Web.Features.Impl
{
    public interface IFeatureSet
    {
        Application Application { get; }
        IIoCContainer Container { get; }
        IReadOnlyCollection<IFeature> Features { get; }

        RequestProcessor GetRequestProcessor();
        RouteInvocation GetRouteInvocation(IRequest request);
    }
}