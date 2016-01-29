using Dolstagis.Web;

namespace WebApp
{
    [Route("/")]
    public class Index : Controller
    {
        public object Get()
        {

            return View("~/views/hello.nustache", Context.Session);
        }
    }
}