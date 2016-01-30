namespace Dolstagis.Web.Features
{
    public interface IContainerUsingExpression<TContainer> where TContainer : IIoCContainer
    {
        IContainerSetupExpression<TContainer> Setup { get; }
    }
}