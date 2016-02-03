namespace Dolstagis.Web.Auth
{
    public interface IRequirement
    {
        bool IsDenied(IRequestContext context);
    }
}
