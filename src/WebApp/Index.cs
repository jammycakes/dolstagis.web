using System.Collections.Generic;
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
            return new {
                message = "Hello world"
            };
            //return new ViewResult("~/views/hello.liquid",
            //    new Dictionary<string, object>() {
            //        { "ID", _session.ID }
            //    }
            //);
        }
    }
}