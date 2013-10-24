using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Http;
using Dolstagis.Web.Lifecycle;

namespace Dolstagis.Web
{
    public interface IRequestContext
    {
        Request Request { get; }

        Response Response { get; }

        ActionInvocation Action { get; }
    }
}
