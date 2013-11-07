using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Http;
using Dolstagis.Web.Lifecycle;
using Dolstagis.Web.Sessions;

namespace Dolstagis.Web
{
    public interface IHttpContext
    {
        IRequestContext Request { get; }

        IResponseContext Response { get; }

        ISession Session { get; }

        IList<ActionInvocation> Actions { get; }
    }
}
