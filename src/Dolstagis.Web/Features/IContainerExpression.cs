namespace Dolstagis.Web.Features
{
    public interface IContainerExpression : IContainerUsingExpression<IIoCContainer>
    {
        IContainerIsExpression<TContainer> Is<TContainer>()
            where TContainer : class, IIoCContainer, new();
    }
}
