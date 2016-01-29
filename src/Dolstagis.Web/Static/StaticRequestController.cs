namespace Dolstagis.Web.Static
{
    public class StaticRequestController : Controller
    {
        public object Get(string path = "")
        {
            return new StaticResult(Context.Request.Path);
        }
    }
}
