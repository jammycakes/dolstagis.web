using Dolstagis.Tests.Web.TestFeatures.Controllers;
using Dolstagis.Web;
using Dolstagis.Web.Features;

namespace Dolstagis.Tests.Web.TestFeatures
{
    public class FirstFeature : Feature
    {
        public FirstFeature()
        {
            Route.From("").To.Controller<RootController>();
            Route.From("one/two").To.Controller<ChildController>();
            Route.From("one/three").To.Controller<ChildController>();
            Route.From("one/two/three/{id}").To.Controller<ChildController>();
            Route.From("one/two/greedy/{id+}").To.Controller<ChildController>();
            Route.From("one/two/optional/{id?}").To.Controller<ChildController>();
            Route.From("one/two/optgreedy/{id*}").To.Controller<ChildController>();
            Route.From("{language}/one/two").To.Controller<LanguageController>();
        }
    }
}
