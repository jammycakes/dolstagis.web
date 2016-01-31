using System;
using System.Collections.Generic;
using System.Linq;
using Dolstagis.Web;
using NUnit.Framework;

namespace Dolstagis.Tests.IoC.Common
{
    [TestFixture]
    public abstract class ContainerFixture<TContainer> where TContainer: IIoCContainer
    {
        protected abstract TContainer CreateContainer();

        private TContainer BuildRegistry(Scope scope, Scope dependencyScope)
        {
            var container = CreateContainer();
            container.Add(typeof(IService), typeof(Service), scope);
            container.Add(typeof(IDependency), typeof(Dependency), dependencyScope);
            return container;
        }

        [TestCase(Scope.Transient, 0, false)]
        [TestCase(Scope.Application, 1, true)]
        [TestCase(Scope.Request, 1, true)]
        public void CanRetrieveASingleInstance
            (Scope scope, int expectedDisposeCount, bool expectSame)
        {
            IService service1, service2;

            using (var container = BuildRegistry(scope, scope)) {
                service1 = container.GetService<IService>();
                service2 = container.GetService<IService>();
            }

            Assert.AreEqual(expectedDisposeCount, service1.DisposeCount);
            Assert.AreEqual(expectedDisposeCount, service2.DisposeCount);
            AssertConditionallySame(expectSame, service1, service2);
        }

        private static void AssertConditionallySame(bool expectSame, IService service1, IService service2)
        {
            if (expectSame)
                Assert.AreSame(service1, service2);
            else
                Assert.AreNotSame(service1, service2);
        }

        [TestCase(Scope.Transient, false, 0, 0)]
        [TestCase(Scope.Application, true, 1, 0)]
        [TestCase(Scope.Request, false, 1, 1)]
        public void CanRetrieveAnInstanceFromAChildContainer
            (Scope scope, bool expectSame, int expectedParentDisposeCount, int expectedChildDisposeCount)
        {
            IService service1, service2;

            using (var container = BuildRegistry(scope, scope)) {
                service1 = container.GetService<IService>();

                using (var childRegistry = container.GetChildContainer()) {
                    service2 = childRegistry.GetService<IService>();
                    AssertConditionallySame(expectSame, service1, service2);
                }
                Assert.AreEqual(0, service1.DisposeCount);
                Assert.AreEqual(expectedChildDisposeCount, service2.DisposeCount);
            }

            Assert.AreEqual(expectedParentDisposeCount, service1.DisposeCount);
            Assert.AreEqual(expectedParentDisposeCount, service2.DisposeCount);
        }


        [Test]
        public void CanRetrieveACollection()
        {
            using (var container = BuildRegistry(Scope.Transient, Scope.Transient)) {
                var service = container.GetService<IEnumerable<IService>>();
                Assert.IsNotNull(service);
                var instance = service.Single();
                Assert.IsNotNull(instance);
            }
        }

        [Test]
        public void CanRetrieveAFactoryMethod()
        {
            using (var container = BuildRegistry(Scope.Transient, Scope.Transient)) {
                var service = container.GetService<Func<IService>>();
                Assert.IsNotNull(service);
                var instance = service();
                Assert.IsNotNull(instance);
            }
        }

        [Test]
        public void CanRetrieveALazy()
        {
            using (var container = BuildRegistry(Scope.Transient, Scope.Transient)) {
                var service = container.GetService<Lazy<IService>>();
                Assert.IsNotNull(service);
                var instance = service.Value;
                Assert.IsNotNull(instance);
            }
        }

        [Test]
        public void GetUnregisteredTypeReturnsNullAndDoesNotThrow()
        {
            using (var container = CreateContainer()) {
                var service = container.GetService<IService>();
                Assert.IsNull(service);
            }
        }

        [Test]
        public void TwoRegistrationsGetTwoServices()
        {
            using (var container = CreateContainer()) {
                container.Add(typeof(IDependency), typeof(Dependency), Scope.Transient);
                container.Add(typeof(IDependency), typeof(Dependency), Scope.Transient);
                var result = container.GetService<IEnumerable<IDependency>>();
                Assert.AreEqual(2, result.Count());
            }
        }

        [Test]
        public void RegistrationsInHierarchyGetTwoServices()
        {
            using (var container = CreateContainer()) {
                container.Add(typeof(IDependency), typeof(Dependency), Scope.Transient);
                using (var child = container.GetChildContainer()) {
                    child.Add(typeof(IDependency), typeof(Dependency), Scope.Transient);
                    var result = child.GetService<IEnumerable<IDependency>>();
                    Assert.AreEqual(2, result.Count());
                }
            }
        }

        [Test]
        public void RequestingAnUnregisteredConcreteServiceReturnsATransientInstance()
        {
            Dependency dep;

            using (var container = CreateContainer()) {
                using (var child = container.GetChildContainer()) {
                    dep = child.GetService<Dependency>();
                    Assert.IsInstanceOf<Dependency>(dep);
                }
            }
            Assert.AreEqual(0, dep.DisposeCount);
        }

        [Test]
        public void RegisteringAnInstanceAlwaysReturnsTheSameInstance()
        {
            Service service = new Service(new Dependency());
            using (var container = CreateContainer()) {
                container.Use(typeof(Service), service);
                var s1 = container.GetService<Service>();
                var s2 = container.GetService<Service>();
                Assert.AreSame(s1, service);
                Assert.AreSame(s2, service);
            }
        }

        [Test]
        public void RegisteringAnInstanceReturnsTheSameInstanceFromAChildRegistry()
        {
            Service service = new Service(new Dependency());
            using (var container = CreateContainer()) {
                container.Use(typeof(Service), service);
                var s1 = container.GetService<Service>();
                using (var child = container.GetChildContainer()) {
                    var s2 = child.GetService<Service>();
                    Assert.AreSame(s1, service);
                    Assert.AreSame(s2, service);
                    Assert.AreSame(s1, s2);
                }
            }
        }

        [TestCase(Scope.Application)]
        [TestCase(Scope.Request)]
        [TestCase(Scope.Transient)]
        public void RegisteringAnInstanceAsADependencyReturnsTheSameInstance(Scope scope)
        {
            IDependency dependency = new Dependency();
            using (var container = CreateContainer()) {
                container.Add(typeof(IService), typeof(Service), scope);
                container.Add(typeof(IDependency), dependency);
                var s1 = container.GetService<IService>();
                using (var child = container.GetChildContainer()) {
                    var s2 = child.GetService<IService>();
                    Assert.AreSame(dependency, s1.Dependency);
                    Assert.AreSame(dependency, s2.Dependency);
                    Assert.AreSame(s1.Dependency, s2.Dependency);
                }
            }
        }


        // [TestCase(Scope.Application)] /* Not supported by StructureMap, probably not by anything */
        [TestCase(Scope.Request)]
        [TestCase(Scope.Transient)]
        public void RegisteringAnInstanceAsADependencyInChildContainerReturnsTheSameInstance(Scope scope)
        {
            IDependency dependency = new Dependency();
            using (var container = CreateContainer()) {
                container.Add(typeof(IService), typeof(Service), scope);
                using (var child = container.GetChildContainer()) {
                    child.Add(typeof(IDependency), dependency);
                    var s = child.GetService<IService>();
                    Assert.AreSame(dependency, s.Dependency);
                }
            }
        }

        [TestCase(Scope.Application)]
        [TestCase(Scope.Request)]
        [TestCase(Scope.Transient)]
        public void RegisteringAnInstanceAsADependencyInParentContainerReturnsTheSameInstance(Scope scope)
        {
            IDependency dependency = new Dependency();
            using (var container = CreateContainer()) {
                container.Add(typeof(IDependency), dependency);
                using (var child = container.GetChildContainer()) {
                    child.Add(typeof(IService), typeof(Service), scope);
                    var s = child.GetService<IService>();
                    Assert.AreSame(dependency, s.Dependency);
                }
            }
        }
    }
}
