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
        public IUser GetUser(IRequestContext context)
        {
            var principal = Thread.CurrentPrincipal;
            return principal.Identity.IsAuthenticated ? new PrincipalUser(principal) : null;
        }

        public void SetUser(IRequestContext context, IUser user)
        {
            throw new NotSupportedException("Changing the logged in user is not supported.");
        }
    }
}
