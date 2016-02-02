using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Dolstagis.Tests.Web.Features.FluentConfiguration
{
    [TestFixture]
    public class ConfigConflictFixture
    {
        [Test]
        public void AddingGlobalConfigurationToFeatureSwitchShouldThrow()
        {
            Assert.Throws<InvalidOperationException>(() => {
                new ConfigConflictFeature(true, true, false);
            });
        }


        [Test]
        public void AddingFeatureSwitchToGlobalConfigurationShouldThrow()
        {
            Assert.Throws<InvalidOperationException>(() => {
                new ConfigConflictFeature(false, true, true);
            });
        }
    }
}
