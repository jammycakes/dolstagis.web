namespace Dolstagis.Web.Features
{
    public interface IRouteDestinationExpression
    {
        IHandlerExpression Handler<THandler>() where THandler : class, new();

        IStaticFilesExpression StaticFiles { get; }
    }
}