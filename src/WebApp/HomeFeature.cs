using System.IO;
using System.Web;
using Dolstagis.Web;
using Dolstagis.Web.Sessions;
using Dolstagis.Web.StructureMap;

namespace WebApp
{
    public class HomeFeature : Feature
    {
        public HomeFeature()
        {
            Description("The home page and static content.");
            Active.When(req => true);

            Container.Is<StructureMapContainer>()
                .Setup.Application(x => {
                    x.Use<ISessionStore, InMemorySessionStore>(Scope.Application);
                })
                .Setup.Feature(x => { })
                .Setup.Request(x => { });

            AddStaticFiles("~/content", Path.Combine(HttpRuntime.AppDomainAppPath, "content"));
            AddViews("~/views", Path.Combine(HttpRuntime.AppDomainAppPath, "views"));
            // Uncomment the following line to use custom error messages.
            // AddViews("~/errors", Path.Combine(HttpRuntime.AppDomainAppPath, "errors"));
            AddController<Index>();
        }
    }
}