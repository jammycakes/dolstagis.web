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
        private HttpContextBase _httpContext;

        public HttpRequestContext(HttpContextBase httpContext)
        {
            _httpContext = httpContext;
        }
    }
}
