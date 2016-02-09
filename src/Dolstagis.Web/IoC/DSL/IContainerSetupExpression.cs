using System;

namespace Dolstagis.Web.IoC.DSL
{
    public interface IContainerSetupExpression<out TContainer> where TContainer : IIoCContainer
    {
        IContainerScopeExpression<TContainer> Application { get; }

        IContainerScopeExpression<TContainer> Feature { get; }

        IContainerScopeExpression<TContainer> Request { get; }
    }
}