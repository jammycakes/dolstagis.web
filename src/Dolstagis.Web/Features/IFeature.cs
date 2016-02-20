using Dolstagis.Web.IoC;
using Dolstagis.Web.Routes;
using Dolstagis.Web.Views;

namespace Dolstagis.Web.Features
{
    public interface IFeature
    {
        int Priority { get; }
        IFeatureSwitch Switch { get; }
        string Description { get; }
        RouteInvocation GetRouteInvocation(VirtualPath path);
        IContainerBuilder ContainerBuilder { get; }
        IModelBinder ModelBinder { get; }
        ViewTable Views { get; }
    }
}
