using System;

namespace Dolstagis.Web.IoC.DSL
{
    public interface IContainerScopeExpression<out TContainer> where TContainer : IIoCContainer
    {
        IContainerUsingExpression<TContainer> Bindings(Action<IBindingExpression> bindings);

        IContainerUsingExpression<TContainer> Container(Action<TContainer> setup);
    }
}
