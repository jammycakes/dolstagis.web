using System;
using System.Collections.Generic;
using System.Reflection;
using Dolstagis.Web;
using Dolstagis.Web.Features;
using Dolstagis.Web.Http;
using Dolstagis.Web.ModelBinding;
using Dolstagis.Web.Routes;
using Dolstagis.Web.StructureMap;
using Moq;
using NUnit.Framework;
using StructureMap;

namespace Dolstagis.Tests.Web.ModelBinding
{
    [TestFixture]
    public class RouteBindingsFixture
    {
        private object methodWithOptionalParameters(string one, string two, string three = "baz")
        {
            return null;
        }

        private object methodWithTypedParameters(int one, bool two, Guid three)
        {
            return null;
        }

        private object methodWithArrayParameters(int[] one)
        {
            return null;
        }

        private IModelBinder binder;

        [OneTimeSetUp]
        public void CreateModelBinder()
        {
            var container = new StructureMapContainer();
            IFeature coreServices = new CoreServices();
            coreServices.ContainerBuilder.SetupApplication(container);
            binder = container.GetService<ModelBinder>();
        }

        [Test]
        public void CanBindRouteData()
        {   
            var data = new Dictionary<string, string>() {
                { "one", "foo" },
                { "two", "bar" }
            };

            var route = new RouteInvocation(null, null, data);
            var request = new Mock<IRequest>();
            request.SetupGet(x => x.Query).Returns(new Dictionary<string, string[]>());

            var method = this.GetType().GetMethod("methodWithOptionalParameters",
                BindingFlags.NonPublic | BindingFlags.Instance);

            var result = binder.GetArguments(route, request.Object, method);

            CollectionAssert.AreEqual(new object[] { "foo", "bar", "baz" }, result);
        }


        [Test]
        public void CanBindRouteDataWithHttpGet()
        {
            var data = new Dictionary<string, string>() {
                { "one", "foo" },
                { "two", "bar" }
            };

            var getData = new Dictionary<string, string[]> {
                { "two", new string[] { "glarch" } },
                { "three", new string[] { "wibble" } }
            };

            var route = new RouteInvocation(null, null, data);
            var request = new Mock<IRequest>();
            request.SetupGet(x => x.Query).Returns(getData);

            var method = this.GetType().GetMethod("methodWithOptionalParameters",
                BindingFlags.NonPublic | BindingFlags.Instance);

            var result = binder.GetArguments(route, request.Object, method);

            CollectionAssert.AreEqual(new object[] { "foo", "glarch", "wibble" }, result);
        }

        [Test]
        public void CanBindRouteDataWithHttpPost()
        {
            var data = new Dictionary<string, string>() {
                { "one", "foo" },
                { "two", "bar" }
            };

            var postData = new Dictionary<string, string[]> {
                { "two", new string[] { "glarch" } },
                { "three", new string[] { "wibble" } }
            };

            var route = new RouteInvocation(null, null, data);
            var request = new Mock<IRequest>();
            request.SetupGet(x => x.Form).Returns(postData);

            var method = this.GetType().GetMethod("methodWithOptionalParameters",
                BindingFlags.NonPublic | BindingFlags.Instance);

            var result = binder.GetArguments(route, request.Object, method);

            CollectionAssert.AreEqual(new object[] { "foo", "glarch", "wibble" }, result);
        }

        [Test]
        public void CanBindRouteDataWithConversions()
        {
            var data = new Dictionary<string, string>() {
                { "one", "1" },
                { "two", "true" },
                { "three", "deadbeef-face-baba-da1e-cafec0deface" }
            };

            var route = new RouteInvocation(null, null, data);
            var request = new Mock<IRequest>();

            var method = this.GetType().GetMethod("methodWithTypedParameters",
                BindingFlags.NonPublic | BindingFlags.Instance);

            var result = binder.GetArguments(route, request.Object, method);

            CollectionAssert.AreEqual(new object[] { 1, true,
                new Guid ("deadbeef-face-baba-da1e-cafec0deface") }, result);
        }

        [Test]
        public void CanBindRouteDataWithArrayConversions()
        {
            var data = new Dictionary<string, string[]> {
                { "one", new string[] { "1", "2", "3" } }
            };

            var route = new RouteInvocation(null, null, new Dictionary<string, string>());
            var request = new Mock<IRequest>();
            request.SetupGet(x => x.Query).Returns(data);

            var method = this.GetType().GetMethod("methodWithArrayParameters",
                BindingFlags.NonPublic | BindingFlags.Instance);

            var result = binder.GetArguments(route, request.Object, method);

            var arr1 = result[0] as int[];

            CollectionAssert.AreEqual(new int[] { 1, 2, 3 }, arr1);
        }
    }
}
