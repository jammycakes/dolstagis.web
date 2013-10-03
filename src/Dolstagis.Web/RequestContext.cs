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
        public RequestContext(IRequest request, IResponse response)
        {
            Request = request;
            Response = response;
        }

        public IRequest Request { get; private set; }

        public IResponse Response { get; private set; }
    }
}
