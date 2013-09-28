using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Dolstagis.Web.Http;

namespace Dolstagis.Web.Aspnet
{
    public class HttpApplication : IHttpApplication
    {
        public HttpApplication(HttpRequestBase request)
        {
            this.Root = request.ApplicationPath.TrimEnd('/') + "/";
            this.PhysicalRoot = request.PhysicalApplicationPath;
        }

        public string Root { get; private set; }

        public string PhysicalRoot { get; private set; }
    }
}
