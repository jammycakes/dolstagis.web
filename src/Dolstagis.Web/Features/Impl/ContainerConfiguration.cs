using System;

namespace Dolstagis.Web.Features.Impl
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
            cb.ConfiguringApplication += (s, e) => {
                if (ConfiguringApplication != null) ConfiguringApplication(s, e);
            };
            cb.SettingContainer += (s, e) => {
                if (SettingContainer != null) SettingContainer(s, e);
            };
        }


        public event EventHandler ConfiguringApplication;

        public event EventHandler SettingContainer;

        public void AssertApplicationNotConfigured(string message)
        {
            if (_builder.ApplicationLevel)
                throw new InvalidOperationException(message);
        }


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
            cb.ConfiguringApplication += (s, e) => {
                if (ConfiguringApplication != null) ConfiguringApplication(s, e);
            };
            cb.SettingContainer += (s, e) => {
                if (SettingContainer != null) SettingContainer(s, e);
            };
            return cb;
        }
    }
}
