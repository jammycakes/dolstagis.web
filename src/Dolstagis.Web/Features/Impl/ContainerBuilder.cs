using System;

namespace Dolstagis.Web.Features.Impl
{
    public class ContainerBuilder<TContainer> : IContainerBuilder,
        IContainerIsExpression<TContainer>,
        IContainerUsingExpression<TContainer>,
        IContainerSetupExpression<TContainer>
        where TContainer : IIoCContainer
    {
        private Action<TContainer> _setupApplicationFunc = container => { };
        private Action<TContainer> _setupDomainFunc = container => { };
        private Action<TContainer> _setupRequestFunc = container => { };


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
            _setupApplicationFunc((TContainer)container);
        }


        public void SetupDomain(IIoCContainer container)
        {
            _setupDomainFunc((TContainer)container);
        }


        public void SetupRequest(IIoCContainer container)
        {
            _setupRequestFunc((TContainer)container);
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

        public IContainerUsingExpression<TContainer> Application(Action<TContainer> setupAction)
        {
            if (ConfiguringApplication != null) ConfiguringApplication(this, EventArgs.Empty);
            ApplicationLevel = true;
            _setupApplicationFunc = setupAction;
            return this;
        }

        public IContainerUsingExpression<TContainer> Feature(Action<TContainer> setupAction)
        {
            _setupDomainFunc = setupAction;
            return this;
        }

        public IContainerUsingExpression<TContainer> Request(Action<TContainer> setupAction)
        {
            _setupRequestFunc = setupAction;
            return this;
        }
    }
}
