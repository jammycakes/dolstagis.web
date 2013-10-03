using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.Lifecycle
{
    public class RequestContextBuilder : IRequestContextBuilder
    {
        public IRequestContext CreateContext(Http.IRequest request, Http.IResponse response)
        {
            return new RequestContext(request, response);
        }
    }
}
