using System;
using StructureMap;
using Dolstagis.Web.IoC;
using System.Collections;
using Dolstagis.Web.Logging;

namespace Dolstagis.Web.StructureMap
{
    public class StructureMapContainer : IIoCContainer<IContainer>
    {
        private static readonly Logger log = Logger.ForThisClass();

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
                x.For<IServiceLocator>().Use(this);
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

        public object Get(Type serviceType)
        {
            if (serviceType.IsAbstract || serviceType.IsInterface ||
                (serviceType.IsGenericType && !serviceType.IsConstructedGenericType))
                return Container.TryGetInstance(serviceType);
            else
                return Container.GetInstance(serviceType);
        }

        public IEnumerable GetAll(Type instanceType)
        {
            return Container.GetAllInstances(instanceType);
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

        public void Validate()
        {
            Container.AssertConfigurationIsValid();
            log.Debug(() => Container.WhatDoIHave());
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
