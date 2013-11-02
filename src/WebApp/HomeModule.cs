using Dolstagis.Web;

namespace WebApp
{
    public class HomeModule : Module
    {
        public HomeModule()
        {
            AddStaticFiles("~/content");
            AddViews("~/views");
            AddViews("~/errors");
            AddHandler<Index>();
        }
    }
}