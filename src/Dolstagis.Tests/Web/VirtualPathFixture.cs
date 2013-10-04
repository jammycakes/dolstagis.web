using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web;
using NUnit.Framework;

namespace Dolstagis.Tests.Web
{
    [TestFixture]
    public class VirtualPathFixture
    {
        [TestCase("/one/two", VirtualPathType.Absolute, "one/two")]
        [TestCase("~/one/two", VirtualPathType.AppRelative, "one/two")]
        [TestCase("one/two", VirtualPathType.RequestRelative, "one/two")]
        [TestCase("../one/two", VirtualPathType.RequestRelative, "../one/two")]
        [TestCase("../one/../two", VirtualPathType.RequestRelative, "../two")]
        [TestCase("../one/../../two", VirtualPathType.RequestRelative, "../../two")]
        [TestCase("~/../one/two", VirtualPathType.AppRelative, "../one/two")]
        [TestCase("/one/two/", VirtualPathType.Absolute, "one/two")]
        public void CanDetectPathType(string path, VirtualPathType type, string expectedPath)
        {
            var vpath = new VirtualPath(path);
            Assert.AreEqual(type, vpath.Type);
            Assert.AreEqual(expectedPath, vpath.Path);
        }

        [TestCase("/one/two", "three/four", VirtualPathType.Absolute, "one/two/three/four")]
        [TestCase("/one/two", "/three/four", VirtualPathType.Absolute, "three/four")]
        [TestCase("one/two", "three/four", VirtualPathType.RequestRelative, "one/two/three/four")]
        [TestCase("one/two", "/three/four", VirtualPathType.Absolute, "three/four")]
        [TestCase("one/two", "../../../three/four", VirtualPathType.RequestRelative, "../three/four")]
        [TestCase("/one/two", "../../../three/four", VirtualPathType.Absolute, "three/four")]
        public void CanAppend(string first, string second, VirtualPathType expectedType, string expectedPath)
        {
            var v1 = new VirtualPath(first);
            var v2 = new VirtualPath(second);
            var actual = v1.Append(v2);
            Assert.AreEqual(expectedType, actual.Type);
            Assert.AreEqual(expectedPath, actual.Path);
        }
    }
}
