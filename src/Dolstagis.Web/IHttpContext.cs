using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Auth;
using Dolstagis.Web.Http;
using Dolstagis.Web.Lifecycle;
using Dolstagis.Web.Sessions;

namespace Dolstagis.Web
{
    public interface IHttpContext
    {
        IRequest Request { get; }

        IResponse Response { get; }

        ISession Session { get; }

        IUser User { get; }

        IList<ActionInvocation> Actions { get; }
    }
}
