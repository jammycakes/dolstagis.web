using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Http;
using NUnit.Framework;

namespace Dolstagis.Tests.Web.Http
{
    [TestFixture]
    public class EncodingsFixture 
    {
        [Test]
        public void CanGetUtf8Encoding()
        {
            var enc = Encodings.Lookup("utf-8");
            Assert.AreSame(Encoding.UTF8, enc);
        }

        [Test]
        public void NonExistentEncodingReturnsNull()
        {
            var enc = Encodings.Lookup(Guid.NewGuid().ToString());
            Assert.IsNull(enc);
        }
    }
}
