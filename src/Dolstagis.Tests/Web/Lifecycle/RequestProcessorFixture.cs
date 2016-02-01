using System;
using System.Threading.Tasks;
using Dolstagis.Tests.Web.TestFeatures;
using Dolstagis.Tests.Web.TestFeatures.Controllers;
using Dolstagis.Web;
using Dolstagis.Web.Features;
using Dolstagis.Web.Features.Impl;
using Dolstagis.Web.Http;
using Dolstagis.Web.Lifecycle;
using Moq;
using NUnit.Framework;

namespace Dolstagis.Tests.Web.Lifecycle
{
    [TestFixture]
    public class RequestProcessorFixture
    {
        private IIoCContainer _mockContainer;

        [OneTimeSetUp]
        public void CreateRouteTable()
        {
            var mock = new Mock<IIoCContainer>();
            mock.Setup(x => x.GetService(It.IsAny<Type>())).Returns(new RootController());
            _mockContainer = mock.Object;
        }


        private async Task<object> Execute(string method, string path)
        {
            var featureSet = new FeatureSet(null, new IFeature[] { new FirstFeature() });
            var processor = new RequestProcessor(null, null, null, null,
                featureSet, 
                () => new ActionInvocation(_mockContainer)
            );
            var request = new Mock<IRequest>();
            request.SetupGet(x => x.Path).Returns(new VirtualPath(path));
            request.SetupGet(x => x.Method).Returns(method);
            var context = await processor.CreateContext(request.Object, null);
            var task = processor.InvokeRequest(context);
            task.Wait();
            return task.Result;
        }


        [Test]
        public async Task CanExecuteSynchronousTask()
        {
            Assert.AreEqual("Hello GET", await Execute("GET", "/"));
        }

        [Test]
        public async Task CanExecuteAsynchronousTaskThatReturnsObject()
        {
            Assert.AreEqual("Hello POST", await Execute("POST", "/"));
        }

        [Test]
        public async Task CanExecuteAsynchronousTaskThatReturnsString()
        {
            Assert.AreEqual("Hello PUT", await Execute("PUT", "/"));
        }

        [Test]
        public async Task CanExecuteAsynchronousTaskThatReturnsTask()
        {
            Assert.AreEqual("Hello DELETE", await Execute("DELETE", "/"));
        }
    }
}
