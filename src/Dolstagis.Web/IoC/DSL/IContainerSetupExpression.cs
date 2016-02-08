using System;

namespace Dolstagis.Web.IoC.DSL
{
    public interface IContainerSetupExpression<out TContainer> where TContainer : IIoCContainer
    {
        IContainerUsingExpression<TContainer> Application(Action<TContainer> setupAction);

        IContainerUsingExpression<TContainer> Feature(Action<TContainer> setupAction);

        IContainerUsingExpression<TContainer> Request(Action<TContainer> setupAction);
    }
}