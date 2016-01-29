using Dolstagis.Tests.Web.TestFeatures.Controllers;
using Dolstagis.Web;

namespace Dolstagis.Tests.Web.TestFeatures
{
    public class FirstFeature : Feature
    {
        public FirstFeature()
        {
            AddController<RootController>();
            AddController<ChildController>("one/two");
            AddController<ChildController>("one/three");
            AddController<ChildController>("one/two/three/{id}");
            AddController<ChildController>("one/two/greedy/{id+}");
            AddController<ChildController>("one/two/optional/{id?}");
            AddController<ChildController>("one/two/optgreedy/{id*}");
            AddController<LanguageController>("{language}/one/two");
        }
    }
}
