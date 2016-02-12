using DotLiquid.FileSystems;

namespace Dolstagis.Web.Views.DotLiquid
{
    public class DotLiquidFeature : Feature
    {
        public DotLiquidFeature()
        {
            Container.Setup.Application.Bindings(bind => {
                bind.From<IViewEngine>().To<DotLiquidViewEngine>().Managed();
                bind.From<IFileSystem>().To<DotLiquidFileSystem>().Managed();
            });
        }
    }
}
