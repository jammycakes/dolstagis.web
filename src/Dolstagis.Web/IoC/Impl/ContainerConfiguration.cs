using System;
using Dolstagis.Web.IoC.DSL;

namespace Dolstagis.Web.IoC.Impl
{
    public class ContainerConfiguration : IContainerExpression
    {
        private IContainerBuilder _builder;
        private IContainerSetupExpression<IIoCContainer> _builderExpression;

        public ContainerConfiguration()
        {
            var cb = new ContainerBuilder<IIoCContainer>();
            _builder = cb;
            _builderExpression = cb;
            cb.SettingContainer += (s, e) => {
                if (SettingContainer != null) SettingContainer(s, e);
            };
        }

        public event EventHandler SettingContainer;

        public void AssertContainerNotSet(string message)
        {
            if (_builder.HasInstance)
                throw new InvalidOperationException(message);
        }


        public IContainerBuilder Builder { get { return _builder; } }

        IContainerSetupExpression<IIoCContainer> IContainerUsingExpression<IIoCContainer>.Setup
        {
            get {
                return _builderExpression;
            }
        }

        IContainerIsExpression<TContainer> IContainerExpression.Is<TContainer>()
        {
            var cb = new ContainerBuilder<TContainer>();
            _builder = cb;
            _builderExpression = cb;
            cb.SettingContainer += (s, e) => {
                if (SettingContainer != null) SettingContainer(s, e);
            };
            return cb;
        }
    }
}
