namespace Dolstagis.Web.Features
{
    public interface IContainerIsExpression<TContainer> where TContainer: IIoCContainer
    {
        IContainerUsingExpression<TContainer> Using(TContainer container);
    }
}