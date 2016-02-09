using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.IoC.DSL;

namespace Dolstagis.Web.IoC.Impl
{
    public class ContainerScope<TContainer> : IContainerScopeExpression<TContainer>
        where TContainer : IIoCContainer
    {
        private ContainerBuilder<TContainer> _owner;
        private Action<TContainer> _setupFunc = container => { };

        public ContainerScope(ContainerBuilder<TContainer> owner)
        {
            _owner = owner;
        }

        public event EventHandler Configuring;

        public void Setup(TContainer container)
        {
            _setupFunc(container);
        }

        IContainerUsingExpression<TContainer>
            IContainerScopeExpression<TContainer>.Container
            (Action<TContainer> setup)
        {
            if (Configuring != null) Configuring(this, EventArgs.Empty);
            _setupFunc = setup;
            return _owner;
        }
    }
}
