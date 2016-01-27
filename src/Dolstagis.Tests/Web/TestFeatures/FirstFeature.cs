using Dolstagis.Tests.Web.TestFeatures.Handlers;
using Dolstagis.Web;

namespace Dolstagis.Tests.Web.TestFeatures
{
    public class FirstFeature : Feature
    {
        public FirstFeature()
        {
            AddHandler<RootHandler>();
            AddHandler<ChildHandler>("one/two");
            AddHandler<ChildHandler>("one/three");
            AddHandler<ChildHandler>("one/two/three/{id}");
            AddHandler<ChildHandler>("one/two/greedy/{id+}");
            AddHandler<ChildHandler>("one/two/optional/{id?}");
            AddHandler<ChildHandler>("one/two/optgreedy/{id*}");
            AddHandler<LanguageHandler>("{language}/one/two");
        }
    }
}
