using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dolstagis.Web.Auth
{
    public class PrincipalAuthenticator : IAuthenticator
    {
        public IUser GetUser(Http.IRequestContext request, Sessions.ISession session)
        {
            var principal = Thread.CurrentPrincipal;
            return principal.Identity.IsAuthenticated ? new PrincipalUser(principal) : null;
        }
    }
}
