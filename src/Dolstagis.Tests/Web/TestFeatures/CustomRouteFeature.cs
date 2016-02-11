using Dolstagis.Tests.Web.TestFeatures.Controllers;
using Dolstagis.Web;
using Dolstagis.Web.Features;

namespace Dolstagis.Tests.Web.TestFeatures
{
    public class CustomRouteFeature : Feature
    {
        public CustomRouteFeature(string customRoute)
        {
            Route.From(customRoute).To.Controller<ChildController>();
        }
    }
}
