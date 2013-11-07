using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Dolstagis.Web.Http;
using Dolstagis.Web.Lifecycle;
using Dolstagis.Web.Sessions;

namespace Dolstagis.Web
{
    public class HttpContext : IHttpContext
    {
        public HttpContext(IRequestContext request, IResponseContext response, ISession session, IEnumerable<ActionInvocation> actions)
        {
            Request = request;
            Response = response;
            Session = session;
            Actions = actions.ToList();
        }

        public IRequestContext Request { get; private set; }

        public IResponseContext Response { get; private set; }

        public ISession Session { get; private set; }

        public IList<ActionInvocation> Actions { get; private set; }

    }
}
