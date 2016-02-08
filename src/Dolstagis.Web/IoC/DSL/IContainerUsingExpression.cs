namespace Dolstagis.Web.IoC.DSL
{
    public interface IContainerUsingExpression<out TContainer> where TContainer : IIoCContainer
    {
        IContainerSetupExpression<TContainer> Setup { get; }
    }
}