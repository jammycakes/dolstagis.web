using System;
using System.Collections.Generic;
using Dolstagis.Web.IoC.DSL;

namespace Dolstagis.Web.IoC.Impl
{
    public class ContainerScope<TContainer> : IContainerScopeExpression<TContainer>,
        IBindingExpression
        where TContainer : IIoCContainer
    {
        private ContainerBuilder<TContainer> _owner;
        private IList<IBinding> _bindings = new List<IBinding>();
        private IList<Action<TContainer>> _setupActions = new List<Action<TContainer>>();

        public ContainerScope(ContainerBuilder<TContainer> owner)
        {
            _owner = owner;
        }

        public event EventHandler Configuring;

        public void Setup(TContainer container)
        {
            foreach (var binding in _bindings) {
                container.Add(binding);
            }

            foreach (var action in _setupActions) {
                action(container);
            }
        }

        IContainerUsingExpression<TContainer>
            IContainerScopeExpression<TContainer>.Container
            (Action<TContainer> setup)
        {
            if (Configuring != null) Configuring(this, EventArgs.Empty);
            _setupActions.Add(setup);
            return _owner;
        }

        IContainerUsingExpression<TContainer>
            IContainerScopeExpression<TContainer>.Bindings
            (Action<IBindingExpression> bindings)
        {
            bindings(this);
            return _owner;
        }

        IFromExpression<TSource> IBindingExpression.From<TSource>()
        {
            var binding = new Binding<TSource>();
            _bindings.Add(binding);
            return binding;
        }
    }
}
