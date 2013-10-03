using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Dolstagis.Web.Http;

namespace Dolstagis.Web
{
    public class RequestContext : IRequestContext
    {
        public RequestContext(IHttpRequest request, IHttpResponse response)
        {
            Request = request;
            Response = response;
        }

        public IHttpRequest Request { get; private set; }

        public IHttpResponse Response { get; private set; }
    }
}
