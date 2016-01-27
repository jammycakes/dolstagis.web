using System;
using Dolstagis.Web.FeatureSwitches;
using NUnit.Framework;

namespace Dolstagis.Tests.Web.FeatureSwitches
{
    [TestFixture]
    public class FeatureSwitchFixture
    {
        [TestCase(-1, DateTimeSwitchType.Activate, true)]
        [TestCase(1, DateTimeSwitchType.Activate, false)]
        [TestCase(-1, DateTimeSwitchType.Deactivate, false)]
        [TestCase(1, DateTimeSwitchType.Deactivate, true)]
        public async void CanSetDateTimeSwitch(int timeOffset, DateTimeSwitchType type, bool expected)
        {
            var dt = DateTime.UtcNow.AddMinutes(timeOffset);
            var @switch = new DateTimeSwitchableAttribute(dt, type);
            var isOn = await @switch.IsEnabledForRequest(null);
            Assert.AreEqual(expected, isOn);
        }
    }
}
