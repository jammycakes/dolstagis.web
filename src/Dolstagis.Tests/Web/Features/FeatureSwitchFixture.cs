using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Tests.Objects;
using Dolstagis.Tests.Objects.Features;
using Dolstagis.Web.StructureMap;
using NUnit.Framework;

namespace Dolstagis.Tests.Web.Features
{
    [TestFixture]
    public class FeatureSwitchFixture
    {
        [Test]
        public void SwitchableFeatureDeclaringAContainerTypeShouldNotThrow()
        {
            Assert.DoesNotThrow(() => new StructureMapFeature(When.Before));
            Assert.DoesNotThrow(() => new StructureMapFeature(When.After));
            Assert.DoesNotThrow(() => new StructureMapFeature(When.Neither));
        }

        [Test]
        public void SwitchableFeatureDeclaringAContainerInstanceShouldThrow()
        {
            var instance = new StructureMapContainer();
            Assert.Throws<InvalidOperationException>(() => new StructureMapFeature(When.Before, instance));
            Assert.Throws<InvalidOperationException>(() => new StructureMapFeature(When.After, instance));
            Assert.DoesNotThrow(() => new StructureMapFeature(When.Neither, instance));
        }
    }
}
