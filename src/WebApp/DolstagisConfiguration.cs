using Dolstagis.Web.Aspnet;

namespace WebApp
{
    public class DolstagisConfiguration : IConfigurator
    {
        public void Configure(Dolstagis.Web.Application application)
        {
            application.ScanForFeatures();
        }
    }
}