namespace Dolstagis.Web.Auth
{
    public interface IAuthenticator
    {
        IUser GetUser(RequestContext context);

        void SetUser(RequestContext context, IUser user);
    }
}
