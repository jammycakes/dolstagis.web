using System;
using Dolstagis.Tests.Web.TestFeatures;
using Dolstagis.Tests.Web.TestFeatures.Controllers;
using Dolstagis.Web;
using Dolstagis.Web.Features;
using Dolstagis.Web.Features.Impl;
using Dolstagis.Web.Http;
using Dolstagis.Web.IoC;
using Dolstagis.Web.Lifecycle;
using Moq;
using NUnit.Framework;

namespace Dolstagis.Tests.Web.Lifecycle
{
    [TestFixture]
    public class RequestProcessorFixture
    {
        private IIoCContainer _mockContainer;
        private ISettings _mockSettings;

        [OneTimeSetUp]
        public void CreateRouteTable()
        {
            var mock = new Mock<IIoCContainer>();
            mock.Setup(x => x.GetService(It.IsAny<Type>())).Returns(new RootController());
            mock.Setup(x => x.GetChildContainer()).Returns<IIoCContainer>(x => x);
            _mockContainer = mock.Object;

            var mockSettings = new Mock<ISettings>();
            mockSettings.Setup(x => x.Debug).Returns(false);
            _mockSettings = mockSettings.Object;
        }


        private object Execute(string method, string path)
        {
            var feature = new FirstFeature();
            var featureSet = new FeatureSet(null, new IFeature[] { feature });
            var processor = new RequestProcessor(null, null, null, null,
                featureSet,
                _mockContainer,
                _mockSettings
            );

            var request = new Mock<IRequest>();
            request.SetupGet(x => x.Path).Returns(new VirtualPath(path));
            request.SetupGet(x => x.Method).Returns(method);
            var context = processor.CreateContext(request.Object, null, _mockContainer);
            var task = context.InvokeRequest();
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
