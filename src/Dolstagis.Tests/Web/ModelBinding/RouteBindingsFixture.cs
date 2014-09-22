using System.Collections.Generic;
using System.Reflection;
using Dolstagis.Web.Http;
using Dolstagis.Web.ModelBinding;
using Dolstagis.Web.Routes;
using Moq;
using NUnit.Framework;

namespace Dolstagis.Tests.Web.ModelBinding
{
    [TestFixture]
    public class RouteBindingsFixture
    {
        private object methodWithOptionalParameters(string one, string two, string three = "baz")
        {
            return null;
        }


        [Test]
        public void CanBindRouteData()
        {
            var binder = new DefaultModelBinder();
            var data = new Dictionary<string, object>() {
                { "one", "foo" },
                { "two", "bar" }
            };

            var route = new RouteInvocation(null, data);
            var request = new Mock<IRequest>();
            request.SetupGet(x => x.Query).Returns(new Dictionary<string, string[]>());

            var method = this.GetType().GetMethod("methodWithOptionalParameters",
                BindingFlags.NonPublic | BindingFlags.Instance);

            var result = binder.GetArguments(route, request.Object, method);

            CollectionAssert.AreEqual(new object[] { "foo", "bar", "baz" }, result);
        }
    }
}
