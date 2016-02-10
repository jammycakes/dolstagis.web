using System;
using System.Collections.Generic;
using System.Linq;
using StructureMap;
using StructureMap.Pipeline;
using Dolstagis.Web.IoC;

namespace Dolstagis.Web.StructureMap
{
    public class StructureMapContainer : IIoCContainer<IContainer>
    {
        private IContainer _container;
        private bool _disposed = false;

        public StructureMapContainer()
            : this(new Container())
        { }

        public StructureMapContainer(IContainer container)
        {
            this._container = container;
            this._container.Configure(x => {
                x.For<IIoCContainer>().Use(this);
                x.For<IServiceProvider>().Use(this);
            });
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
            return new StructureMapFeatureContainer(_container.CreateChildContainer());
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

        public virtual void Add(IBinding binding)
        {
            Container.Configure(x => {
                var from = x.For(binding.SourceType);
                if (binding.TargetType != null) {
                    var to = binding.Multiple
                    ? from.Add(binding.TargetType)
                    : from.Use(binding.TargetType);
                    if (binding.Transient) to.Transient(); else to.Singleton();
                }
                else if (binding.TargetFunc != null) {
                    var to = binding.Multiple
                        ? from.Add(ctr => binding.TargetFunc(this))
                        : from.Use(ctr => binding.TargetFunc(this));
                    if (binding.Transient) to.Transient(); else to.Singleton();
                }
                else if (binding.Target != null) {
                    var to = binding.Multiple
                        ? from.Add(binding.Target)
                        : from.Use(binding.Target);
                    if (binding.Transient) to.Transient(); else to.Singleton();
                }
            });
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

        public void Validate()
        {
            Container.AssertConfigurationIsValid();
        }


        /* ====== StructureMap specifics ====== */

        public void AddRegistry(Registry registry)
        {
            this.Container.Configure(x => x.AddRegistry(registry));
        }

        public void AddRegistry<TRegistry>() where TRegistry: Registry, new()
        {
            this.Container.Configure(x => x.AddRegistry<TRegistry>());
        }

        private class StructureMapFeatureContainer : StructureMapContainer
        {
            public StructureMapFeatureContainer(IContainer container)
                : base(container)
            { }

            public override IIoCContainer GetChildContainer()
            {
                return new StructureMapRequestContainer(_container.GetNestedContainer(), this);
            }
        }

        private class StructureMapRequestContainer : StructureMapContainer
        {
            private StructureMapFeatureContainer _parent;

            public StructureMapRequestContainer(IContainer container, StructureMapFeatureContainer parent)
                : base(container)
            {
                _parent = parent;
            }

            public override IIoCContainer GetChildContainer()
            {
                return new StructureMapRequestContainer(_container.GetNestedContainer(), _parent);
            }

            public override void Add(IBinding binding)
            {
                if (binding.Transient) {
                    base.Add(binding);
                }
                else {
                    _parent.Container.Configure(x => {
                        var from = x.For(binding.SourceType).ContainerScoped();
                        if (binding.TargetType != null) {
                            var to = binding.Multiple
                            ? from.Add(binding.TargetType)
                            : from.Use(binding.TargetType);
                        }
                        else if (binding.TargetFunc != null) {
                            var to = binding.Multiple
                                ? from.Add(ctr => binding.TargetFunc(this))
                                : from.Use(ctr => binding.TargetFunc(this));
                        }
                        else if (binding.Target != null) {
                            var to = binding.Multiple
                                ? from.Add(binding.Target)
                                : from.Use(binding.Target);
                        }
                    });
                }
            }
        }
    }
}
