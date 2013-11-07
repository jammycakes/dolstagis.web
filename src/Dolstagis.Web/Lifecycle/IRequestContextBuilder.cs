using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Http;

namespace Dolstagis.Web.Lifecycle
{
    public interface IRequestContextBuilder
    {
        IHttpContext CreateContext(Request request, Response response);
    }
}
