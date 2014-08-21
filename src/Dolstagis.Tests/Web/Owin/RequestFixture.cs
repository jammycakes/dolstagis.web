using System;
using System.Collections.Generic;
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
    }
}
