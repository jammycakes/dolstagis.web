using Dolstagis.Web.Static;
using Dolstagis.Web.Features;

namespace Dolstagis.Web._dolstagis
{
    internal class DolstagisFeature : Feature
    {
        public DolstagisFeature()
        {
            string ns = this.GetType().Namespace;

            Route("~/_dolstagis").To.StaticFiles
                .FromAssemblyResourcesRelativeTo<DolstagisFeature>();
        }
    }
}
