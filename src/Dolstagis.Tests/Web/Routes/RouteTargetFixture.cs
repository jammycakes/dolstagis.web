using System;
using Dolstagis.Web.IoC;
using Dolstagis.Web.Routes;
using Moq;
using NUnit.Framework;

namespace Dolstagis.Tests.Web.Routes
{
    [TestFixture]
    public class RouteTargetFixture
    {
        [Test]
        public void CanCreateRouteTargetFromType()
        {
            object obj = new object();

            var mockServiceProvider = new Mock<IServiceLocator>();
            mockServiceProvider.Setup(x => x.Get(typeof(object))).Returns(obj);

            Type t = typeof(object);

            var target = new RouteTarget(x => x.Get(typeof(object)));

            Assert.AreEqual(typeof(object), target.ControllerType);
            Assert.AreSame(obj, target.GetController(mockServiceProvider.Object));
        }
    }
}
