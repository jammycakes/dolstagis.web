using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Tests.Web.TestFeatures;
using Dolstagis.Tests.Web.TestFeatures.Handlers;
using Dolstagis.Web;
using Dolstagis.Web.Http;
using Dolstagis.Web.Lifecycle;
using Dolstagis.Web.ModelBinding;
using Moq;
using NUnit.Framework;
using StructureMap;

namespace Dolstagis.Tests.Web.Lifecycle
{
    [TestFixture]
    public class RequestProcessorFixture
    {
        private IContainer _mockContainer;

        [TestFixtureSetUp]
        public void CreateRouteTable()
        {
            var mock = new Mock<IContainer>();
            mock.Setup(x => x.GetInstance(It.IsAny<Type>())).Returns(new RootHandler());
            _mockContainer = mock.Object;
        }


        private object Execute(string method, string path)
        {
            var featureSet = new FeatureSet(null, new Feature[] { new FirstFeature() });
            var builder = new RequestContextBuilder(
                null, 
                null, 
                featureSet,
                () => new ActionInvocation(_mockContainer),
                new DefaultModelBinder(new IConverter[0])
            );
            var processor = new RequestProcessor(null, null, builder);
            var request = new Mock<IRequest>();
            request.SetupGet(x => x.Path).Returns(new VirtualPath(path));
            request.SetupGet(x => x.Method).Returns(method);
            var context = builder.CreateContext(request.Object, null);
            var task = processor.InvokeRequest(context);
            task.Wait();
            return task.Result;
        }


        [Test]
        public void CanExecuteSynchronousTask()
        {
            Assert.AreEqual("Hello GET", Execute("GET", "/"));
        }

        [Test]
        public void CanExecuteAsynchronousTaskThatReturnsObject()
        {
            Assert.AreEqual("Hello POST", Execute("POST", "/"));
        }

        [Test]
        public void CanExecuteAsynchronousTaskThatReturnsString()
        {
            Assert.AreEqual("Hello PUT", Execute("PUT", "/"));
        }

        [Test]
        public void CanExecuteAsynchronousTaskThatReturnsTask()
        {
            Assert.AreEqual("Hello DELETE", Execute("DELETE", "/"));
        }
    }
}
