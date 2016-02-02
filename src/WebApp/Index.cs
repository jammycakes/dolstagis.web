using Dolstagis.Web;
using Dolstagis.Web.Sessions;
using Dolstagis.Web.Views;

namespace WebApp
{
    [Route("/")]
    public class Index
    {
        private ISession _session;

        public Index(ISession session)
        {
            _session = session;
        }

        public object Get()
        {
            return new ViewResult("~/views/hello.nustache", _session);
        }
    }
}