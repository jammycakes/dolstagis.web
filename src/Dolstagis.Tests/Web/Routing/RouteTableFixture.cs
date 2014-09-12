using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Tests.Web.TestFeatures;
using Dolstagis.Tests.Web.TestFeatures.Handlers;
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
            var routeTable = new RouteTable(new FirstFeature());
            routeTable.RebuildRouteTable();

            var one = routeTable.Root.GetMatchingChildren("one");
            Assert.AreEqual("one", one.Single().Name);
            Assert.IsNotInstanceOf<ParameterEntry>(one.Single());
            CollectionAssert.IsEmpty(one.Single().Definitions);
        }

        [Test]
        public void CanGetParameterChildNode()
        {
            var routeTable = new RouteTable(new FirstFeature());
            routeTable.RebuildRouteTable();

            var two = routeTable.Root.GetMatchingChildren("two");
            Assert.AreEqual("{language}", two.Single().Name);
            Assert.IsInstanceOf<ParameterEntry>(two.Single());
            CollectionAssert.IsEmpty(two.Single().Definitions);
        }

        [Test]
        public void CanGetRoot()
        {
            var routeTable = new RouteTable(new FirstFeature());
            var route = routeTable.Lookup("/");
            Assert.AreEqual(typeof(RootHandler), route.Definition.HandlerType);
        }

        [Test]
        public void CanGetExactRoute()
        {
            var routeTable = new RouteTable(new FirstFeature());
            var route = routeTable.Lookup("/one/two");
            Assert.AreEqual(typeof(ChildHandler), route.Definition.HandlerType);
        }

        [Test]
        public void CanGetParametrisedRouteWhenOnlyOne()
        {
            var routeTable = new RouteTable(new FirstFeature());
            var route = routeTable.Lookup("/de-DE/one/two");
            Assert.AreEqual(typeof(LanguageHandler), route.Definition.HandlerType);
            Assert.AreEqual(1, route.Arguments.Count);
            Assert.AreEqual("de-DE", route.Arguments["language"]);
        }

        [Test]
        public void DoesNotGetRouteWhenNoMatches()
        {
            var routeTable = new RouteTable(new FirstFeature());
            Assert.IsNull(routeTable.Lookup("/de-DE/"));
            Assert.IsNull(routeTable.Lookup("/de-DE/one/wibble"));
            Assert.IsNull(routeTable.Lookup("/de-DE/one/two/wibble/wobble"));
        }

        [Test]
        public void CanGetRouteThatEndsWithParameter()
        {
            var routeTable = new RouteTable(new FirstFeature());
            var route = routeTable.Lookup("/one/two/three/1");
            Assert.AreEqual(typeof(ChildHandler), route.Definition.HandlerType);
            Assert.AreEqual("1", route.Arguments["id"]);
        }

        [Test]
        public void DoesNotGetRouteWithParameterMissing()
        {
            var routeTable = new RouteTable(new FirstFeature());
            Assert.IsNull(routeTable.Lookup("/one/two/three"));
        }

        [Test]
        public void DoesNotGetRouteWithTooManyParameters()
        {
            var routeTable = new RouteTable(new FirstFeature());
            Assert.IsNull(routeTable.Lookup("/one/two/three/four/five"));
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GreedyParametersMustComeLast()
        {
            var routeTable = new RouteTable(new CustomRouteFeature("/one/two/{id*}/{name?}"));
            routeTable.RebuildRouteTable();
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void OptionalParametersMustNotBeFollowedByNonOptionalParameters()
        {
            var routeTable = new RouteTable(new CustomRouteFeature("/one/two/{id?}/{name+}"));
            routeTable.RebuildRouteTable();
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void OptionalParametersMustNotBeFollowedByExactMatches()
        {
            var routeTable = new RouteTable(new CustomRouteFeature("/one/two/{id?}/three"));
            routeTable.RebuildRouteTable();
        }


        [Test]
        public void CanHaveMultipleOptionalParameters()
        {
            var routeTable = new RouteTable(new CustomRouteFeature("/one/two/{id?}/{name*}"));
            routeTable.RebuildRouteTable();
        }

        [Test]
        public void ErrorInBuildingRouteTableDoesNotSetRoot()
        {
            RouteTable routeTable = null;
            try {
                routeTable = new RouteTable(new CustomRouteFeature("/one/two/{id?}/three"));
                routeTable.RebuildRouteTable();
            }
            catch (InvalidOperationException) {
                Assert.IsNull(routeTable.Root);
                return;
            }
            Assert.Fail();
        }

        [Test]
        public void CanGetRouteWithGreedyParameter()
        {
            var routeTable = new RouteTable(new FirstFeature());
            var route = routeTable.Lookup("/one/two/greedy/three/four/five");
            Assert.AreEqual("three/four/five", route.Arguments["id"]);
            Assert.IsNull(routeTable.Lookup("/one/two/greedy"));
        }

        [Test]
        public void CanGetRouteWithOptionalParameter()
        {
            var routeTable = new RouteTable(new FirstFeature());
            var route = routeTable.Lookup("/one/two/optional/three");
            Assert.AreEqual("three", route.Arguments["id"]);
            Assert.IsNotNull(routeTable.Lookup("/one/two/optional"));
            Assert.IsNull(routeTable.Lookup("/one/two/optional/three/four/five"));
        }

        [Test]
        public void CanGetRouteWithOptionalGreedyParameter()
        {
            var routeTable = new RouteTable(new FirstFeature());
            var route = routeTable.Lookup("/one/two/optgreedy/three/four/five");
            Assert.AreEqual("three/four/five", route.Arguments["id"]);
            Assert.IsNotNull(routeTable.Lookup("/one/two/optgreedy"));
        }

        [Test]
        public void VerifyOptionalParametersAreNotStored()
        {
            var routeTable = new RouteTable(new FirstFeature());
            var route = routeTable.Lookup("/one/two/optional");
            Assert.IsFalse(route.Arguments.ContainsKey("id"));
        }
    }
}
