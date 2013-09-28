using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Dolstagis.Web.Http;

namespace Dolstagis.Web.Aspnet
{
    public class HttpRequestContext : IHttpContext
    {
        public HttpRequestContext(HttpContextBase httpContext)
        {
            Request = new HttpRequest(httpContext.Request);
            Response = new HttpResponse(httpContext.Response);
            Application = new HttpApplication(httpContext.Request);
        }

        public IHttpRequest Request { get; private set; }

        public IHttpResponse Response { get; private set; }

        public IHttpApplication Application { get; private set; }
    }
}
