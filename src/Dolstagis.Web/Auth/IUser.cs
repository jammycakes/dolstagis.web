namespace Dolstagis.Web.Auth
{
    public interface IUser
    {
        string UserName { get; }

        bool IsInRole(string role);
    }
}
