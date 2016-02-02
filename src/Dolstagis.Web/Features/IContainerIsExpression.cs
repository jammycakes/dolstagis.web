namespace Dolstagis.Web.Features
{
    public interface IContainerIsExpression<TContainer> 
        : IContainerUsingExpression<TContainer>
        where TContainer: IIoCContainer
    {
        IContainerUsingExpression<TContainer> Using(TContainer container);
    }
}