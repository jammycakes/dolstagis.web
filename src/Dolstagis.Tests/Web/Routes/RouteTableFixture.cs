using System;
using Dolstagis.Web;
using Dolstagis.Web.Routes;
using Moq;
using NUnit.Framework;

namespace Dolstagis.Tests.Web.Routes
{
    [TestFixture]
    public class RouteTableFixture
    {
        private RouteTable routes = new RouteTable(null);
        private IRouteTarget rootTarget = Mock.Of<IRouteTarget>();
        private IRouteTarget oneTwo = Mock.Of<IRouteTarget>();
        private IRouteTarget oneThree = Mock.Of<IRouteTarget>();
        private IRouteTarget withId = Mock.Of<IRouteTarget>();
        private IRouteTarget withGreedyId = Mock.Of<IRouteTarget>();
        private IRouteTarget withOptionalId = Mock.Of<IRouteTarget>();
        private IRouteTarget withOptGreedyId = Mock.Of<IRouteTarget>();
        private IRouteTarget withLanguage = Mock.Of<IRouteTarget>();


        [TestFixtureSetUp]
        public void CreateRouteTable()
        {
            routes.Add(String.Empty, rootTarget);

            routes.Add("one/two", oneTwo);
            routes.Add("one/three", oneThree);
            routes.Add("one/two/three/{id}", withId);
            routes.Add("one/two/greedy/{id+}", withGreedyId);
            routes.Add("one/two/optional/{id?}", withOptionalId);
            routes.Add("one/two/optgreedy/{id*}", withOptGreedyId);
            routes.Add("{id}/one/two", withLanguage);
        }


        /* ====== CanGetMatchingRouteData ====== */

        /*
         * First check that valid routes match and give the correct parameters
         */

        [TestCase("~/", null)]
        [TestCase("~/one/two", null)]
        [TestCase("~/one/three", null)]
        [TestCase("~/one/two/three/four", "four")]
        [TestCase("~/one/two/greedy/four", "four")]
        [TestCase("~/one/two/greedy/four/five/six", "four/five/six")]
        [TestCase("~/one/two/optional", null)]
        [TestCase("~/one/two/optional/four", "four")]
        [TestCase("~/one/two/optgreedy", null)]
        [TestCase("~/one/two/optgreedy/four", "four")]
        [TestCase("~/one/two/optgreedy/four/five/six", "four/five/six")]
        [TestCase("~/en-gb/one/two", "en-gb")]
        public void CanGetMatchingRouteData(string path, string expectedId)
        {
            var invocation = routes.GetRouteInvocation(new VirtualPath(path));
            string id;
            if (!invocation.RouteData.TryGetValue("id", out id)) id = null;
            Assert.AreEqual(expectedId, id);
        }


        /* ====== DoesNotGetNonMatchingRouteData ====== */

        /*
         * Now test some routes that should not match.
         */

        [TestCase("~/one")]                     // intermediate route with no target
        [TestCase("~/one/four")]                // nonexistent route
        [TestCase("~/zero/four")]               // nonexistent route
        [TestCase("~/one/two/three")]           // required route data is missing
        [TestCase("~/one/two/three/four/five")] // required nongreedy has too many parts
        [TestCase("~/one/two/greedy")]          // required greedy route data is missing
        [TestCase("~/one/two/optional/3/4/5")]  // optional nongreedy has too many parts
        [TestCase("~/en-gb/one/three/")]        // initial parameter but the rest doesn't match
        public void DoesNotGetNonMatchingRouteData(string path)
        {
            var target = routes.GetRouteInvocation(new VirtualPath(path));
            Assert.IsNull(target);
        }


        /* ====== Tests for the correct targets ====== */

        /*
         * The following tests verify that the correct target is returned.
         */

        [Test]
        public void CanGetRootTarget()
        {
            var root = routes.GetRouteInvocation(new VirtualPath("~/"));
            Assert.AreSame(rootTarget, root.Target);
        }

        [Test]
        public void CanGetOneTwoTarget()
        {
            var invocation = routes.GetRouteInvocation(new VirtualPath("~/one/two"));
            Assert.AreSame(oneTwo, invocation.Target);
        }

        [Test]
        public void CanGetParametrisedTarget()
        {
            var invocation = routes.GetRouteInvocation(new VirtualPath("~/one/two/three/4"));
            Assert.AreSame(withId, invocation.Target);
        }
    }
}
