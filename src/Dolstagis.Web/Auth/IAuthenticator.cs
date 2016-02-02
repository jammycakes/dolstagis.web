using System.Threading.Tasks;
using Dolstagis.Web.Lifecycle;

namespace Dolstagis.Web.Auth
{
    public interface IAuthenticator
    {
        Task<IUser> GetUser(RequestContext context);

        Task SetUser(RequestContext context, IUser user);
    }
}
