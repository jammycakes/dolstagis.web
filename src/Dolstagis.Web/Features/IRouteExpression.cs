namespace Dolstagis.Web.Features
{
    public interface IRouteExpression
    {
        IRouteDestinationExpression To { get; }
    }
}