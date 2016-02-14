using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Http;
using NUnit.Framework;

namespace Dolstagis.Tests.Web.Http.Response
{
    [TestFixture]
    public class ContentTypeFixture
    {
        private ResponseHeaders CreateHeaders()
        {
            return new ResponseHeaders
                (new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase));
        }

        [Test]
        public void SettingTheMimeTypeSetsTheContentType()
        {
            var dict = CreateHeaders();
            dict.MimeType = "text/html";
            Assert.AreEqual("text/html", dict.ContentType);
            Assert.AreEqual("text/html", dict["Content-Type"].Single());
            Assert.AreEqual("text/html", dict.MimeType);
        }

        [Test]
        public void SettingTheEncodingSetsTheContentType()
        {
            var dict = CreateHeaders();
            dict.MimeType = "application/json";
            dict.Encoding = Encoding.UTF8;
            Assert.AreEqual("application/json;charset=utf-8", dict.ContentType);
            Assert.AreEqual("application/json;charset=utf-8", dict["Content-Type"].Single());
            Assert.AreSame(Encoding.UTF8, dict.Encoding);
        }

        [Test]
        public void SettingTheContentTypeSetsTheEncodingAndTheMimeType()
        {
            var dict = CreateHeaders();
            dict.ContentType = "  application/xml  ; charset=utf-7  ";
            Assert.AreEqual("application/xml", dict.MimeType);
            Assert.AreSame(Encoding.UTF7, dict.Encoding);
        }
    }
}
