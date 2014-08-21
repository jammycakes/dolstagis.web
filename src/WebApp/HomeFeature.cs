using System.IO;
using System.Web;
using Dolstagis.Web;
using Dolstagis.Web.Sessions;

namespace WebApp
{
    public class HomeFeature : Feature
    {
        public HomeFeature()
        {
            Services.For<ISessionStore>().Singleton().Use<InMemorySessionStore>();

            AddStaticFiles("~/content", Path.Combine(HttpRuntime.AppDomainAppPath, "content"));
            AddViews("~/views", Path.Combine(HttpRuntime.AppDomainAppPath, "views"));
            // Uncomment the following line to use custom error messages.
            // AddViews("~/errors", Path.Combine(HttpRuntime.AppDomainAppPath, "errors"));
            AddHandler<Index>();
        }
    }
}