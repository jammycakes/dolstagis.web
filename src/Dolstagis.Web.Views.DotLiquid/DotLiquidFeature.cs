using DotLiquid.FileSystems;

namespace Dolstagis.Web.Views.DotLiquid
{
    public class DotLiquidFeature : Feature
    {
        public DotLiquidFeature()
        {
            Container.Setup.Application(c => {
                c.Add<IViewEngine, DotLiquidViewEngine>(Scope.Application);
                c.Add<IFileSystem, DotLiquidFileSystem>(Scope.Application);
            });
        }
    }
}
