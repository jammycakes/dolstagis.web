using Dolstagis.Web;
using Dolstagis.Web.Sessions;

namespace WebApp
{
    public class HomeFeature : Feature
    {
        public HomeFeature()
        {
            Services.For<ISessionStore>().Singleton().Use<InMemorySessionStore>();

            AddStaticFiles("~/content");
            AddViews("~/views");
            // Uncomment the following line to use custom error messages.
            // AddViews("~/errors");
            AddHandler<Index>();
        }
    }
}