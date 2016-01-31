namespace Dolstagis.Web.Features
{
    public interface IContainerUsingExpression<out TContainer> where TContainer : IIoCContainer
    {
        IContainerSetupExpression<TContainer> Setup { get; }
    }
}