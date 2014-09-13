using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Owin;
using NUnit.Framework;

namespace Dolstagis.Tests.Web.Owin
{
    [TestFixture]
    public class RequestFixture : OwinFixtureBase
    {

        [Test]
        public void CanParseQueryString()
        {
            string queryString = "?one=two&three=four&five=six&one=seven";
            var environment = BuildDefaultOwinEnvironment();
            environment[EnvironmentKeys.RequestQueryString] = queryString;
            var request = new Request(environment);
            Assert.AreEqual(3, request.Query.Count);
            CollectionAssert.AreEquivalent(new string[] { "two", "seven" }, request.Query["one"]);
        }

        [Test]
        public void CanParseBlankForm()
        {
            var environment = BuildDefaultOwinEnvironment();
            var request = new Request(environment);
            Assert.IsNotNull(request.Form);
            Assert.IsEmpty(request.Form);
        }

        [Test]
        public void CanParsePostedForm()
        {
            string form = "one=two&three=four&five=six&one=seven";
            var bytes = Encoding.UTF8.GetBytes(form);
            using (var stream = new MemoryStream(bytes))
            {
                var environment = BuildDefaultOwinEnvironment();
                environment[EnvironmentKeys.RequestBody] = stream;
                ((IDictionary<string, string[]>)environment[EnvironmentKeys.RequestHeaders])
                    ["Content-Type"] = new string[] { "application/x-www-form-urlencoded" };
                var request = new Request(environment);
                Assert.IsNotNull(request.Form);
                CollectionAssert.AreEquivalent
                    (new string[] { "one", "three", "five" }, request.Form.Keys);
                CollectionAssert.AreEquivalent
                    (new string[] { "two", "seven" }, request.Form["one"]);
            }
        }
    }
}
