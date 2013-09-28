using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Routing;
using NUnit.Framework;

namespace Dolstagis.Tests.Web.Routing
{
    [TestFixture]
    public class RoutingHelpersFixture
    {
        [TestCase("one/two", "one/two")]
        [TestCase("one/../two", "two")]
        [TestCase("../two", "two")]
        [TestCase("one/..", "")]
        [TestCase("..", "")]
        public void CanNormalisePath(string path, string expected)
        {
            Assert.AreEqual(expected, path.NormaliseUrlPath());
        }
    }
}
