using Dolstagis.Web;

namespace WebApp
{
    [Route("/")]
    public class Index : Handler
    {
        public object Get()
        {
            return View("~/views/hello.nustache", new { Message = "Hello from Nustache" });
        }
    }
}