using System;
using Dolstagis.Tests.Web.TestFeatures;
using Dolstagis.Tests.Web.TestFeatures.Controllers;
using Dolstagis.Web;
using Dolstagis.Web.Features;
using Dolstagis.Web.Features.Impl;
using Dolstagis.Web.Http;
using Dolstagis.Web.Lifecycle;
using Moq;
using NUnit.Framework;
using StructureMap;

namespace Dolstagis.Tests.Web.Lifecycle
{
    [TestFixture]
    public class RequestProcessorFixture
    {
        private IContainer _mockContainer;

        [OneTimeSetUp]
        public void CreateRouteTable()
        {
            var mock = new Mock<IContainer>();
            mock.Setup(x => x.GetInstance(It.IsAny<Type>())).Returns(new RootController());
            _mockContainer = mock.Object;
        }


        private object Execute(string method, string path)
        {
            var featureSet = new FeatureSet(null, new IFeature[] { new FirstFeature() });
            var processor = new RequestProcessor(null, null, null, null,
                featureSet, 
                () => new ActionInvocation(_mockContainer)
            );
            var request = new Mock<IRequest>();
            request.SetupGet(x => x.Path).Returns(new VirtualPath(path));
            request.SetupGet(x => x.Method).Returns(method);
            var context = processor.CreateContext(request.Object, null);
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
