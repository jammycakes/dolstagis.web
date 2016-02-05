using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            var mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider.Setup(x => x.GetService(typeof(object))).Returns(obj);

            Type t = typeof(object);

            var target = new RouteTarget(x => x.GetService(typeof(object)));

            Assert.AreEqual(typeof(object), target.ControllerType);
            Assert.AreSame(obj, target.GetController(mockServiceProvider.Object));
        }
    }
}
