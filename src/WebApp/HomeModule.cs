using Dolstagis.Web;
using Dolstagis.Web.Sessions;

namespace WebApp
{
    public class HomeModule : Module
    {
        public HomeModule()
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