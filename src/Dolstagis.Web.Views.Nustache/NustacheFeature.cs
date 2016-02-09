using Dolstagis.Web.IoC;

namespace Dolstagis.Web.Views.Nustache
{
    public class NustacheFeature : Feature
    {
        public NustacheFeature()
        {
            Container.Setup.Application.Container(c => {
                c.Add<IViewEngine, NustacheViewEngine>(Scope.Application);
            });
        }
    }
}
