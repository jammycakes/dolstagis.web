using Dolstagis.Web.IoC;

namespace Dolstagis.Web.Views.Nustache
{
    public class NustacheFeature : Feature
    {
        public NustacheFeature()
        {
            Container.Setup.Application.Bindings(bind => {
                bind.From<IViewEngine>().To<NustacheViewEngine>().Managed();
            });
        }
    }
}
