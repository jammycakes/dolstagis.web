using Dolstagis.Web.Routes;

namespace Dolstagis.Web.Features
{
    public interface IFeature : ILegacyFeature
    {
        IFeatureSwitch Switch { get; }
        string Description { get; }
        RouteInvocation GetRouteInvocation(VirtualPath path);
        IContainerBuilder ContainerBuilder { get; }
    }
}
