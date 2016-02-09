using System.IO;
using System.Web;
using Dolstagis.Web;
using Dolstagis.Web.Aspnet;
using Dolstagis.Web.IoC;
using Dolstagis.Web.Sessions;
using Dolstagis.Web.StructureMap;

namespace WebApp
{
    public class HomeFeature : Feature
    {
        public HomeFeature()
        {
            Description("The home page and static content.");
            Container.Is<StructureMapContainer>()
                .Setup.Application(x => {
                    x.Use<ISessionStore, InMemorySessionStore>(Scope.Application);
                })
                .Setup.Feature(x => { })
                .Setup.Request(x => { });

            // This is how you would set up a feature switch. Note that you can't
            // have both a feature switch and application-level IOC configuration
            // in the same feature.
            // Active.When(req => true);

            //Route("~/").To.Controller<Index>();
            Route("~/content").To.StaticFiles.FromWebApplication("~/content");

            AddViews("~/views", Path.Combine(HttpRuntime.AppDomainAppPath, "views"));
            // Uncomment the following line to use custom error messages.
            // AddViews("~/errors", Path.Combine(HttpRuntime.AppDomainAppPath, "errors"));
        }
    }
}