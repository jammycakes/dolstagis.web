using Dolstagis.Web.Lifecycle;

namespace Dolstagis.Web.Auth
{
    public interface IRequirement
    {
        bool IsDenied(RequestContext context);
    }
}
