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
                .Setup.Application.Bindings(bind => {
                    bind.From<ISessionStore>().Only().To<InMemorySessionStore>().Managed();
                });

            // This is how you would set up a feature switch. Note that you can't
            // have both a feature switch and application-level IOC configuration
            // in the same feature.
            // Active.When(req => true);

            //Route.From("~/").To.Controller<Index>();
            Route.From("~/content").To.StaticFiles.FromWebApplication("~/content");

            AddViews("~/views", Path.Combine(HttpRuntime.AppDomainAppPath, "views"));
            // Uncomment the following line to use custom error messages.
            // AddViews("~/errors", Path.Combine(HttpRuntime.AppDomainAppPath, "errors"));
        }
    }
}