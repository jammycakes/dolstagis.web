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

        [TestCase("/one/two", "/one/two/three/four", false, "three/four")]
        [TestCase("~/one/two", "~/one/two/three/four", false, "three/four")]
        [TestCase("/one/two", "/ONE/TWO/three/four", true, "three/four")]
        [TestCase("/one/two", "/ONE/TWO/three/four", false, null)]
        [TestCase("one/two", "one/two/three/four", false, null)]
        [TestCase("/one/two", "~/one/two/three/four", false, null)]
        [TestCase("~/one/two", "/one/two/three/four", false, null)]
        [TestCase("/one/two", "/one/three/three/four", false, null)]
        public void CanGetSubPath(string first, string second, bool ignoreCase, string expected)
        {
            var v1 = new VirtualPath(first);
            var v2 = new VirtualPath(second);
            var diff = v1.GetSubPath(v2, ignoreCase);
            if (expected == null) {
                Assert.IsNull(diff);
            }
            else {
                Assert.AreEqual(expected, diff.ToString());
            }



        }
    }
}
