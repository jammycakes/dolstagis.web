using System;
using System.Threading;
using System.Threading.Tasks;
using Dolstagis.Web.Lifecycle;

namespace Dolstagis.Web.Auth
{
    public class PrincipalAuthenticator : IAuthenticator
    {
        public Task<IUser> GetUser(RequestContext context)
        {
            var principal = Thread.CurrentPrincipal;
            return Task.FromResult<IUser>
                (principal.Identity.IsAuthenticated ? new PrincipalUser(principal) : null);
        }

        public Task SetUser(RequestContext context, IUser user)
        {
            throw new NotSupportedException("Changing the logged in user is not supported.");
        }
    }
}
