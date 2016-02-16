using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Tests.Objects;
using Dolstagis.Tests.Objects.Fakes;
using Dolstagis.Tests.Objects.Features;
using Dolstagis.Tests.Objects.Services;
using Dolstagis.Web;
using Dolstagis.Web.Http;
using Dolstagis.Web.IoC;
using Dolstagis.Web.IoC.DSL;
using Dolstagis.Web.StructureMap;
using Moq;
using NUnit.Framework;

namespace Dolstagis.Tests.Web.Features
{
    [TestFixture]
    public class ApplicationFixture
    {
        private Feature _switchableOn;
        private Feature _switchableOff;
        private Feature _nonSwitchable;

        private Action<IContainerExpression> ConfigureFeature(string serviceName)
        {
            return container => {
                var mockService = new Mock<IService>();
                mockService.Setup(x => x.Name).Returns(serviceName);
                container.Setup.Application.Bindings(bind => {
                    bind.From<IService>().To(mockService.Object);
                });
            };
        }


        private Application CreateApplication(params Feature[] features)
        {
            var application = new Application(Mock.Of<ISettings>());
            foreach (var feature in features) application.AddFeature(feature);
            application.Configure();
            return application;
        }


        [OneTimeSetUp]
        public void CreateFeatures()
        {
            _switchableOff = new SwitchableFeature(false, ConfigureFeature("Off"));
            _switchableOn = new SwitchableFeature(true, ConfigureFeature("On"));
            _nonSwitchable = new NonSwitchableFeature(ConfigureFeature("NonSwitchable"));
        }

        [Test]
        public void AtLeastOneFeatureMustSpecifyAContainer()
        {
            Assert.Throws<InvalidOperationException>(() => {
                CreateApplication(_switchableOff, _switchableOn, _nonSwitchable);
            });
        }

        [Test]
        public void AtMostOneExplicitContainerMayBeSpecified()
        {
            Assert.Throws<InvalidOperationException>(() => {
                CreateApplication(
                    new ContainerFeature<FakeIoCContainer>(instance: new FakeIoCContainer()),
                    new ContainerFeature<FakeIoCContainer>(instance: new FakeIoCContainer())
                );
            });
        }

        [Test]
        public void AtMostOneExplicitContainerMayBeSpecifiedWithSwitchableFeatures()
        {
            Assert.Throws<InvalidOperationException>(() => {
                CreateApplication(
                    new ContainerFeature<FakeIoCContainer>(instance: new FakeIoCContainer()),
                    new ContainerFeature<FakeIoCContainer>(When.After, new FakeIoCContainer())
                );
            });
        }


        [Test]
        public void AllImplicitContainersMustBeTheSameType()
        {
            Assert.Throws<InvalidOperationException>(() => {
                CreateApplication(
                    new ContainerFeature<FakeIoCContainer<String>>(),
                    new ContainerFeature<FakeIoCContainer<int>>()
                );
            });
        }

        [Test]
        public void TheSameExplicitContainerCanBeSpecifiedMultipleTimes()
        {
            Assert.DoesNotThrow(() => {
                var instance = new FakeIoCContainer();
                CreateApplication(
                    new ContainerFeature<FakeIoCContainer>(instance: instance),
                    new ContainerFeature<FakeIoCContainer>(instance: instance)
                );
            });
        }

        [Test]
        public void ExplicitContainerMustBeSpecifiedInAtLeastOneNonSwitchableFeature()
        {
            Assert.Throws<InvalidOperationException>(() => {
                var instance = new FakeIoCContainer();
                CreateApplication(
                    new ContainerFeature<FakeIoCContainer>(When.Before, instance),
                    new ContainerFeature<FakeIoCContainer>()
                );
            });
        }

        [Test]
        public void ImplicitContainerMustBeSpecifiedInAtLeastOneNonSwitchableFeature()
        {
            Assert.Throws<InvalidOperationException>(() => {
                CreateApplication(
                    new ContainerFeature<FakeIoCContainer>(When.Before),
                    new NonSwitchableFeature(x => { })
                );
            });
        }


        [Test]
        public void ApplicationContainerReturnsOnlyNonSwitchableServices()
        {
            var containerFeature = new ContainerFeature<StructureMapContainer>();
            var application = CreateApplication(containerFeature, _nonSwitchable, _switchableOff, _switchableOn);
            var services = application.Container.GetAll<IService>();
            Assert.AreEqual("NonSwitchable", services.Single().Name);
        }


        [Test]
        public void FeatureContainerReturnsSwitchableAndUnswitchableFeatures()
        {
            // TODO: Ensure that standard IOC tests (elsewhere) require both parent and child registrations to be returned

            var containerFeature = new ContainerFeature<StructureMapContainer>();
            var application = CreateApplication(containerFeature, _nonSwitchable, _switchableOff, _switchableOn);
            var request = Mock.Of<IRequest>();
            var features = application.Features.GetFeatureSet(request);
            var services = features.Container.GetAll<IService>();
            CollectionAssert.AreEquivalent(
                new string[] { "On", "NonSwitchable" },
                services.Select(x => x.Name)
            );
        }
    }
}
