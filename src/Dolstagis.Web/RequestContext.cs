using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Dolstagis.Web.Http;
using Dolstagis.Web.Lifecycle;

namespace Dolstagis.Web
{
    public class RequestContext : IRequestContext
    {
        public RequestContext(IRequest request, IResponse response, ActionInvocation action)
        {
            Request = request;
            Response = response;
            Action = action;
        }

        public IRequest Request { get; private set; }

        public IResponse Response { get; private set; }

        public ActionInvocation Action { get; private set; }
    }
}
