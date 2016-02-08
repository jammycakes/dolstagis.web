namespace Dolstagis.Web.IoC.DSL
{
    public interface IContainerExpression : IContainerUsingExpression<IIoCContainer>
    {
        IContainerIsExpression<TContainer> Is<TContainer>()
            where TContainer : class, IIoCContainer, new();
    }
}
