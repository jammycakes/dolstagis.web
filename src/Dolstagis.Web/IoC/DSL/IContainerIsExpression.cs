namespace Dolstagis.Web.IoC.DSL
{
    public interface IContainerIsExpression<TContainer> 
        : IContainerUsingExpression<TContainer>
        where TContainer: IIoCContainer
    {
        IContainerUsingExpression<TContainer> Using(TContainer container);
    }
}