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
            AddViews("~/errors");
            AddHandler<Index>();
        }
    }
}