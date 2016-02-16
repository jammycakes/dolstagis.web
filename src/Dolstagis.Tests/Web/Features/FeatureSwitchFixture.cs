using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Tests.Objects;
using Dolstagis.Tests.Objects.Fakes;
using Dolstagis.Tests.Objects.Features;
using Dolstagis.Web.IoC;
using Dolstagis.Web.StructureMap;
using Moq;
using NUnit.Framework;

namespace Dolstagis.Tests.Web.Features
{
    [TestFixture]
    public class FeatureSwitchFixture
    {
        [Test]
        public void SwitchableFeatureDeclaringAContainerTypeShouldNotThrow()
        {
            Assert.DoesNotThrow(() => new ContainerFeature<FakeIoCContainer>(When.Before));
            Assert.DoesNotThrow(() => new ContainerFeature<FakeIoCContainer>(When.After));
            Assert.DoesNotThrow(() => new ContainerFeature<FakeIoCContainer>(When.Neither));
        }

        [Test]
        public void SwitchableFeatureDeclaringAContainerInstanceShouldThrow()
        {
            var instance = new FakeIoCContainer();
            Assert.Throws<InvalidOperationException>(() => new ContainerFeature<FakeIoCContainer>(When.Before, instance));
            Assert.Throws<InvalidOperationException>(() => new ContainerFeature<FakeIoCContainer>(When.After, instance));
            Assert.DoesNotThrow(() => new ContainerFeature<FakeIoCContainer>(When.Neither, instance));
        }
    }
}
