using System;
using Dolstagis.Web.Aspnet;
using Dolstagis.Web.Views.DotLiquid;

namespace WebApp
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            // By default we scan all assemblies for features. However, we can
            // override this by scanning manually for extra startup performance.

            Startup.ConfigureApplication(x => {
                x.AddAllFeaturesInAssemblyOf<Global>();
                x.AddFeature<DotLiquidFeature>();
            });
        }
    }
}