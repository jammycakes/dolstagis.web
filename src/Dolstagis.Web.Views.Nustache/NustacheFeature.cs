namespace Dolstagis.Web.Views.Nustache
{
    public class NustacheFeature : Feature
    {
        public NustacheFeature()
        {
            this.Services.For<IViewEngine>().Singleton().Add<NustacheViewEngine>();
        }
    }
}
