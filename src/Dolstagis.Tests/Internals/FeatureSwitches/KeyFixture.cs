using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web;
using Dolstagis.Web.Lifecycle;
using NUnit.Framework;

namespace Dolstagis.Tests.Internals.FeatureSwitches
{
    [TestFixture]
    public class KeyFixture
    {
        private IEnumerable<bool> GetRandomBits(int count)
        {
            var random = new Random();

            for (var i = 0; i < count; i++) {
                yield return random.Next(2) != 0;
            }
        }

        [Test]
        public void CanCompareKeys()
        {
            foreach (var b in new bool[] { false, true }) {
                for (var i = 29; i <= 34; i++) {
                    var b0 = new bool[] { false };
                    var b1 = new bool[] { true };

                    var entropy = GetRandomBits(i);
                        var key1 = new FeatureSwitchboard.Key
                            (b ? b0.Concat(entropy) : entropy.Concat(b0));
                        var key2 = new FeatureSwitchboard.Key
                            (b ? b1.Concat(entropy) : entropy.Concat(b1));

                    Assert.True(b0.Equals(b0));
                    Assert.True(b1.Equals(b1));
                    Assert.False(b0.Equals(b1));
                    Assert.False(b1.Equals(b0));
                }
            }
        }
    }
}
