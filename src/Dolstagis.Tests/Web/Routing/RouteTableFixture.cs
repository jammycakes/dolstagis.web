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
            Assert.IsNotInstanceOf<ParameterEntry>(one.Single());
            Assert.IsNull(one.Single().Definition);
        }

        [Test]
        public void CanGetParameterChildNode()
        {
            var routeTable = new RouteTable(new FirstModule());
            routeTable.RebuildRouteTable();

            var two = routeTable.Root.GetMatchingChildren("two");
            Assert.AreEqual("{language}", two.Single().Name);
            Assert.IsInstanceOf<ParameterEntry>(two.Single());
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

        [Test]
        public void DoesNotGetRouteFromDisabledModules()
        {
            var routeTable = new RouteTable(new FirstModule(), new DisabledModule());
            Assert.IsNull(routeTable.Lookup("/foo/bar"));
        }

        [Test]
        public void CanGetRouteThatEndsWithParameter()
        {
            var routeTable = new RouteTable(new FirstModule());
            var route = routeTable.Lookup("/one/two/three/1");
            Assert.AreEqual(typeof(ChildHandler), route.Definition.HandlerType);
            Assert.AreEqual("1", route.Arguments["id"]);
        }

        [Test]
        public void DoesNotGetRouteWithParameterMissing()
        {
            var routeTable = new RouteTable(new FirstModule());
            Assert.IsNull(routeTable.Lookup("/one/two/three"));
        }

        [Test]
        public void DoesNotGetRouteWithTooManyParameters()
        {
            var routeTable = new RouteTable(new FirstModule());
            Assert.IsNull(routeTable.Lookup("/one/two/three/four/five"));
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GreedyParametersMustComeLast()
        {
            var routeTable = new RouteTable(new CustomRouteModule("/one/two/{id*}/{name?}"));
            routeTable.RebuildRouteTable();
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void OptionalParametersMustNotBeFollowedByNonOptionalParameters()
        {
            var routeTable = new RouteTable(new CustomRouteModule("/one/two/{id?}/{name+}"));
            routeTable.RebuildRouteTable();
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void OptionalParametersMustNotBeFollowedByExactMatches()
        {
            var routeTable = new RouteTable(new CustomRouteModule("/one/two/{id?}/three"));
            routeTable.RebuildRouteTable();
        }


        [Test]
        public void CanHaveMultipleOptionalParameters()
        {
            var routeTable = new RouteTable(new CustomRouteModule("/one/two/{id?}/{name*}"));
            routeTable.RebuildRouteTable();
        }

        [Test]
        public void ErrorInBuildingRouteTableDoesNotSetRoot()
        {
            RouteTable routeTable = null;
            try {
                routeTable = new RouteTable(new CustomRouteModule("/one/two/{id?}/three"));
                routeTable.RebuildRouteTable();
            }
            catch (InvalidOperationException) {
                Assert.IsNull(routeTable.Root);
                return;
            }
            Assert.Fail();
        }

        [Test]
        [Ignore("TODO: not yet implemented")]
        public void CanGetRouteWithGreedyParameter()
        {
            var routeTable = new RouteTable(new FirstModule());
            var route = routeTable.Lookup("/one/two/greedy/three/four/five");
            Assert.AreEqual("three/four/five", route.Arguments["id"]);
            Assert.IsNull(routeTable.Lookup("/one/two/greedy"));
        }

        [Test]
        [Ignore("TODO: not yet implemented")]
        public void CanGetRouteWithOptionalParameter()
        {
            var routeTable = new RouteTable(new FirstModule());
            var route = routeTable.Lookup("/one/two/optional/three");
            Assert.AreEqual("three", route.Arguments["id"]);
            Assert.IsNotNull(routeTable.Lookup("/one/two/optional"));
            Assert.IsNull(routeTable.Lookup("/one/two/optional/three/four/five"));
        }

        [Test]
        [Ignore("TODO: not yet implemented")]
        public void CanGetRouteWithOptionalGreedyParameter()
        {
            var routeTable = new RouteTable(new FirstModule());
            var route = routeTable.Lookup("/one/two/optgreedy/three/four/five");
            Assert.AreEqual("three/four/five", route.Arguments["id"]);
            Assert.IsNotNull(routeTable.Lookup("/one/two/optgreedy"));
        }

        [Test]
        [Ignore("TODO: not yet implemented")]
        public void VerifyOptionalParametersAreNotStored()
        {
            var routeTable = new RouteTable(new FirstModule());
            var route = routeTable.Lookup("/one/two/optional");
            Assert.IsFalse(route.Arguments.ContainsKey("id"));
        }
    }
}
