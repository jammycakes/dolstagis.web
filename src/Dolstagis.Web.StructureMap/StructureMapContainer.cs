using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StructureMap;
using StructureMap.Pipeline;

namespace Dolstagis.Web.StructureMap
{
    public class StructureMapContainer : IIoCContainer<IContainer>
    {
        private IContainer _container;
        private bool _disposed = false;

        public StructureMapContainer()
        {
            this._container = new Container();
        }

        public StructureMapContainer(IContainer container)
        {
            this._container = container;
        }

        public IContainer Container
        {
            get {
                return _container;
            }
        }

        public void Dispose()
        {
            if (!_disposed) {
                _container.Dispose();
                _disposed = true;
            }
        }

        public virtual IIoCContainer GetChildContainer()
        {
            return new StructureMapDomainContainer(_container.CreateChildContainer());
        }

        public object GetService(Type serviceType)
        {
            if (serviceType.IsGenericType && serviceType.GetGenericTypeDefinition() == typeof(IEnumerable<>)) {
                var itemType = serviceType.GetGenericArguments().FirstOrDefault();
                if (Container.Model.HasImplementationsFor(itemType)) {
                    return Container.GetInstance(serviceType);
                }
            }

            if (serviceType.IsAbstract || serviceType.IsInterface ||
                (serviceType.IsGenericType && !serviceType.IsConstructedGenericType))
                return Container.TryGetInstance(serviceType);
            else
                return Container.GetInstance(serviceType);
        }

        public void Add(Type source, Type target, Scope scope)
        {
            Container.Configure(x => {
                var entry = x.For(source).Add(target);
                SetScope(scope, entry);
            });
        }

        public void Use(Type source, Type target, Scope scope)
        {
            Container.Configure(x => {
                var entry = x.For(source).Use(target);
                SetScope(scope, entry);
            });
        }

        private static void SetScope(Scope scope, ConfiguredInstance entry)
        {
            switch (scope) {
                case Scope.Transient:
                    entry.Transient();
                    break;
                case Scope.Application:
                    entry.Singleton();
                    break;
                case Scope.Request:
                    entry.ContainerScoped();
                    break;
            }
        }

        private static void SetScope(Scope scope, LambdaInstance<object> entry)
        {
            switch (scope) {
                case Scope.Transient:
                    entry.Transient();
                    break;
                case Scope.Application:
                    entry.Singleton();
                    break;
                case Scope.Request:
                    entry.ContainerScoped();
                    break;
            }
        }

        public void Add(Type source, Func<IIoCContainer, object> target, Scope scope)
        {
            Container.Configure(x => {
                var entry = x.For(source).Add(ctx => target(ctx.GetInstance<IIoCContainer>()));
                SetScope(scope, entry);
            });
        }

        public void Use(Type source, Func<IIoCContainer, object> target, Scope scope)
        {
            Container.Configure(x => {
                var entry = x.For(source).Use(ctx => target(ctx.GetInstance<IIoCContainer>()));
                SetScope(scope, entry);
            });
        }

        public void Add(Type source, object target)
        {
            Container.Configure(x => {
                var entry = x.For(source).Add(target);
                entry.Transient();
            });
        }

        public void Use(Type source, object target)
        {
            Container.Configure(x => {
                var entry = x.For(source).Use(target);
                entry.Transient();
            });
        }

        private class StructureMapDomainContainer : StructureMapContainer
        {
            public StructureMapDomainContainer(IContainer container)
                : base(container)
            { }

            public override IIoCContainer GetChildContainer()
            {
                return new StructureMapContainer(_container.CreateChildContainer());
            }
        }
    }
}
