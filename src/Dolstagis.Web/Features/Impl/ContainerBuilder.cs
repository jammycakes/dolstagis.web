using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.Features.Impl
{
    public class ContainerBuilder<TContainer> : IContainerBuilder,
        IContainerIsExpression<TContainer>,
        IContainerUsingExpression<TContainer>,
        IContainerSetupExpression<TContainer>
        where TContainer : class, IIoCContainer, new()
    {
        private TContainer _instance = null;
        private Action<TContainer> _setupApplicationFunc = container => { };
        private Action<TContainer> _setupDomainFunc = container => { };
        private Action<TContainer> _setupRequestFunc = container => { };


        /* ====== IContainerBuilder implementation ====== */

        private const string configurationError =
            "Conflicting IOC container configurations detected. " +
            "Please make sure that the features in your application are all " +
            "configured to use the same IOC container.";

        public IIoCContainer GetContainer(IIoCContainer existing)
        {
            if (existing == null) return _instance ?? new TContainer();
            if (_instance == null) return existing;

            if (!(existing is TContainer)) {
                throw new InvalidOperationException(configurationError);
            }

            if (!Object.ReferenceEquals(_instance, existing)) {
                throw new InvalidOperationException(configurationError);
            }

            return existing;
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


        /* ====== Fluent configuration interfaces implementation ====== */

        public IContainerUsingExpression<TContainer> Using(TContainer container)
        {
            _instance = container;
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
            _setupApplicationFunc = setupAction;
            return this;
        }

        public IContainerUsingExpression<TContainer> Domain(Action<TContainer> setupAction)
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
