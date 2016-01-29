using System;
using System.Threading;
using Dolstagis.Web.Lifecycle;

namespace Dolstagis.Web.Auth
{
    public class PrincipalAuthenticator : IAuthenticator
    {
        public IUser GetUser(RequestContext context)
        {
            var principal = Thread.CurrentPrincipal;
            return principal.Identity.IsAuthenticated ? new PrincipalUser(principal) : null;
        }

        public void SetUser(RequestContext context, IUser user)
        {
            throw new NotSupportedException("Changing the logged in user is not supported.");
        }
    }
}
