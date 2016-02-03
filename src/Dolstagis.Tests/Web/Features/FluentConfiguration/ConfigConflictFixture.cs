using System;
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
