using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Tests.Web.TestModules;
using Dolstagis.Tests.Web.TestModules.Handlers;
using Dolstagis.Web.Routing;
using NUnit.Framework;

namespace Dolstagis.Tests.Web.Routing
{
    [TestFixture]
    public class RouteTableFixture
    {
        [Test]
        public void CanGetExactChildNode()
        {
            var routeTable = new RouteTable(new FirstModule());
            routeTable.RebuildRouteTable();

            var one = routeTable.Root.GetMatchingChildren("one");
            Assert.AreEqual("one", one.Single().Name);
            Assert.IsFalse(one.Single().IsParameter);
            Assert.IsNull(one.Single().Definition);
        }

        [Test]
        public void CanGetParameterChildNode()
        {
            var routeTable = new RouteTable(new FirstModule());
            routeTable.RebuildRouteTable();

            var two = routeTable.Root.GetMatchingChildren("two");
            Assert.AreEqual("{language}", two.Single().Name);
            Assert.IsTrue(two.Single().IsParameter);
            Assert.IsNull(two.Single().Definition);
        }

        [Test]
        public void CanGetRoot()
        {
            var routeTable = new RouteTable(new FirstModule());
            var route = routeTable.Lookup("/");
            Assert.AreEqual(typeof(RootHandler), route.Definition.HandlerType);
        }

        [Test]
        public void CanGetExactRoute()
        {
            var routeTable = new RouteTable(new FirstModule());
            var route = routeTable.Lookup("/one/two");
            Assert.AreEqual(typeof(ChildHandler), route.Definition.HandlerType);
        }

        [Test]
        public void CanGetParametrisedRouteWhenOnlyOne()
        {
            var routeTable = new RouteTable(new FirstModule());
            var route = routeTable.Lookup("/de-DE/one/two");
            Assert.AreEqual(typeof(LanguageHandler), route.Definition.HandlerType);
            Assert.AreEqual(1, route.Arguments.Count);
            Assert.AreEqual("de-DE", route.Arguments["language"]);
        }

        [Test]
        public void DoesNotGetRouteWhenNoMatches()
        {
            var routeTable = new RouteTable(new FirstModule());
            Assert.IsNull(routeTable.Lookup("/de-DE/"));
            Assert.IsNull(routeTable.Lookup("/de-DE/one/wibble"));
            Assert.IsNull(routeTable.Lookup("/de-DE/one/two/wibble/wobble"));
        }
    }
}
