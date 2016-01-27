using Dolstagis.Tests.Web.TestFeatures.Handlers;
using Dolstagis.Web;

namespace Dolstagis.Tests.Web.TestFeatures
{
    public class CustomRouteFeature : Feature
    {
        public CustomRouteFeature(string customRoute)
        {
            AddHandler<ChildHandler>(customRoute);
        }
    }
}
