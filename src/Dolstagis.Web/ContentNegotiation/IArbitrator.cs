using Dolstagis.Web.Http;

namespace Dolstagis.Web.ContentNegotiation
{
    public interface IArbitrator
    {
        IResult Arbitrate(IRequest request, object model);
    }
}