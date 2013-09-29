using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Http;
using Dolstagis.Web.Routing;

namespace Dolstagis.Web.Lifecycle
{
    public class RequestProcessor : IRequestProcessor
    {
        private RouteTable _routes;

        public RequestProcessor(RouteTable routes)
        {
            _routes = routes;
        }

        public Task ProcessRequest(IHttpContext context)
        {
            return Task.Run(() => {});
        }
    }
}
