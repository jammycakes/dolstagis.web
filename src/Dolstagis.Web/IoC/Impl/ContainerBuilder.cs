using System;
using Dolstagis.Web.IoC.DSL;


namespace Dolstagis.Web.IoC.Impl
{
    public class ContainerBuilder<TContainer> : IContainerBuilder,
        IContainerIsExpression<TContainer>,
        IContainerUsingExpression<TContainer>,
        IContainerSetupExpression<TContainer>
        where TContainer : IIoCContainer
    {
        private ContainerScope<TContainer> _application;
        private ContainerScope<TContainer> _feature;
        private ContainerScope<TContainer> _request;

        public ContainerBuilder()
        {
            _application = new ContainerScope<TContainer>(this);
            _feature = new ContainerScope<TContainer>(this);
            _request = new ContainerScope<TContainer>(this);

            _application.Configuring += (s, e) => {
                ApplicationLevel = true;
                if (ConfiguringApplication != null)
                    ConfiguringApplication(this, EventArgs.Empty);
            };
        }


        /* ====== IContainerBuilder implementation ====== */

        public bool HasInstance { get; private set; }

        public IIoCContainer Instance { get; private set; }

        public bool ApplicationLevel { get; private set; }

        public Type ContainerType
        {
            get { return typeof(TContainer); }
        }

        public IIoCContainer CreateContainer()
        {
            return Activator.CreateInstance<TContainer>();
        }


        public void SetupApplication(IIoCContainer container)
        {
            _application.Setup((TContainer)container);
        }


        public void SetupFeature(IIoCContainer container)
        {
            _feature.Setup((TContainer)container);
        }


        public void SetupRequest(IIoCContainer container)
        {
            _request.Setup((TContainer)container);
        }

        public event EventHandler ConfiguringApplication;
        public event EventHandler SettingContainer;


        /* ====== Fluent configuration interfaces implementation ====== */

        public IContainerUsingExpression<TContainer> Using(TContainer container)
        {
            if (SettingContainer != null) SettingContainer(this, EventArgs.Empty);
            Instance = container;
            HasInstance = true;
            return this;
        }

        public IContainerSetupExpression<TContainer> Setup
        {
            get {
                return this;
            }
        }

        public IContainerScopeExpression<TContainer> Application
        {
            get { return _application; }
        }

        public IContainerScopeExpression<TContainer> Feature
        {
            get { return _feature; }
        }

        public IContainerScopeExpression<TContainer> Request
        {
            get { return _request; }
        }
    }
}
