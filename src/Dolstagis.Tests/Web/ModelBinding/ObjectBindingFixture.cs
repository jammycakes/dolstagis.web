using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web;
using Dolstagis.Web.Http;
using Dolstagis.Web.ModelBinding;
using Dolstagis.Web.Routes;
using Moq;
using NUnit.Framework;
using StructureMap;

namespace Dolstagis.Tests.Web.ModelBinding
{
    [TestFixture]
    public class ObjectBindingFixture
    {
        private class TestObject
        {
            public int IntValue { get; set; }

            public string StringValue { get; set; }

            public DateTime DateTimeValue { get; set; }
        }

        private object testBindingMethod(TestObject test)
        {
            return null;
        }

        private IModelBinder binder;

        [TestFixtureSetUp]
        public void CreateModelBinder()
        {
            var container = new Container();
            container.Configure(x => x.AddRegistry<CoreServices>());
            binder = container.GetInstance<DefaultModelBinder>();
        }


        [Test]
        public void CanBindCompleteTestObject()
        {
            var data = new Dictionary<string, string>() {
                { "IntValue", "1" },
                { "StringValue", "bar" },
                { "DateTimeValue", "2014-01-01" }
            };

            var route = new RouteInvocation(null, data);
            var request = new Mock<IRequest>();
            request.SetupGet(x => x.Query).Returns(new Dictionary<string, string[]>());

            var method = this.GetType().GetMethod("testBindingMethod",
                BindingFlags.NonPublic | BindingFlags.Instance);

            var result = binder.GetArguments(route, request.Object, method);
            var obj = result.Single() as TestObject;

            Assert.IsNotNull(obj);
            Assert.AreEqual(1, obj.IntValue);
            Assert.AreEqual("bar", obj.StringValue);
            Assert.AreEqual(new DateTime(2014, 1, 1), obj.DateTimeValue);
        }
    }
}
