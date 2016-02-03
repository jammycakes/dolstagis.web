using System.Threading.Tasks;

namespace Dolstagis.Web.Auth
{
    public interface IAuthenticator
    {
        Task<IUser> GetUser(IRequestContext context);

        Task SetUser(IRequestContext context, IUser user);
    }
}
