using Dolstagis.Tests.Web.TestFeatures.Controllers;
using Dolstagis.Web;
using Dolstagis.Web.Features;

namespace Dolstagis.Tests.Web.TestFeatures
{
    public class FirstFeature : Feature
    {
        public FirstFeature()
        {
            Route("").To.Controller<RootController>();
            Route("one/two").To.Controller<ChildController>();
            Route("one/three").To.Controller<ChildController>();
            Route("one/two/three/{id}").To.Controller<ChildController>();
            Route("one/two/greedy/{id+}").To.Controller<ChildController>();
            Route("one/two/optional/{id?}").To.Controller<ChildController>();
            Route("one/two/optgreedy/{id*}").To.Controller<ChildController>();
            Route("{language}/one/two").To.Controller<LanguageController>();
        }
    }
}
