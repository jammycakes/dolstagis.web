using System;

namespace Dolstagis.Web.Features
{
    public interface IContainerSetupExpression<out TContainer> where TContainer : IIoCContainer
    {
        IContainerUsingExpression<TContainer> Application(Action<TContainer> setupAction);

        IContainerUsingExpression<TContainer> Domain(Action<TContainer> setupAction);

        IContainerUsingExpression<TContainer> Request(Action<TContainer> setupAction);
    }
}