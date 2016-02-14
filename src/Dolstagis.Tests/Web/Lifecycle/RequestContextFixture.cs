using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Tests.Web.TestFeatures.Controllers;
using Dolstagis.Web;
using Dolstagis.Web.Features;
using Dolstagis.Web.Features.Impl;
using Dolstagis.Web.Http;
using Dolstagis.Web.Lifecycle;
using Dolstagis.Web.Routes;
using Moq;
using NUnit.Framework;

namespace Dolstagis.Tests.Web.Lifecycle
{
    [TestFixture]
    public class RequestContextFixture
    {
        private RequestContext CreateRequestContext<TException>(bool async, bool afterAwait)
            where TException : Exception, new()
        {
            var features = new Mock<IFeatureSet>();
            var feature = new Mock<IFeature>();
            var modelBinder = new Mock<IModelBinder>();
            var request = new Mock<IRequest>();

            var controller = new ThrowingController<TException>(async, afterAwait);
            var target = new RouteTarget(ioc => controller);
            var routeInvocation = new RouteInvocation(feature.Object, target, new Dictionary<string, string>());

            request.Setup(x => x.Method).Returns("GET");

            modelBinder.Setup(
                x => x.GetArguments(
                    It.IsAny<RouteInvocation>(), It.IsAny<IRequest>(),
                    It.IsAny<MethodInfo>())
                )
                .Returns(new object[0]);

            feature.Setup(x => x.ModelBinder).Returns(modelBinder.Object);
            features.Setup(x => x.GetRouteInvocation(It.IsAny<IRequest>()))
                .Returns(routeInvocation);

            return new RequestContext(request.Object, null, null, null, null, features.Object);
        }


        [TestCase(false, false)]
        [TestCase(true, false)]
        [TestCase(true, true)]
        public async Task ThrowingThrowsTheCorrectException(bool throwAsync, bool afterAwait)
        {
            var context = CreateRequestContext<InvalidOperationException>(throwAsync, afterAwait);
            try {
                await context.InvokeRequest();
            }
            catch (InvalidOperationException ex) {
                var trace = new StackTrace(ex);
                Assert.Greater(trace.FrameCount, 3);
            }
        }
    }
}