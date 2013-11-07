using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Tests.Web.Lifecycle.AmbiguousRouteModules;
using Dolstagis.Web;
using Dolstagis.Web.Http;
using Dolstagis.Web.Lifecycle;
using Dolstagis.Web.Routing;
using Moq;
using NUnit.Framework;
using StructureMap;

namespace Dolstagis.Tests.Web.Lifecycle
{
    [TestFixture]
    public class AmbiguousRoutesFixture
    {
        private RouteTable _routeTable;
        private IContainer _container;

        [TestFixtureSetUp]
        public void CreateRouteTable()
        {
            _routeTable = new RouteTable(new FirstModule(), new SecondModule());
            _routeTable.RebuildRouteTable();
            _container = new Container();
        }

        private object Execute(string method, string path)
        {
            var builder = new RequestContextBuilder(_routeTable, null, () => new ActionInvocation(_container));
            var processor = new RequestProcessor(null, null, builder);
            var request = new Mock<IRequest>();
            request.SetupGet(x => x.AppRelativePath).Returns(new VirtualPath(path));
            request.SetupGet(x => x.Method).Returns(method);
            var context = builder.CreateContext(new Request(request.Object), null);
            var task = processor.InvokeRequest(context);
            task.Wait();
            return task.Result;
        }

        [Test]
        public void InvokingFirstRouteReturnsFirst()
        {
            Assert.AreEqual("First", Execute("GET", "/HandledByFirst"));
        }


        [Test]
        public void InvokingSecondRouteReturnsSecond()
        {
            Assert.AreEqual("Second", Execute("GET", "/HandledBySecond"));
        }

        [Test]
        public void FavourSpecificOverArguments()
        {
            Assert.AreEqual("First", Execute("GET", "/args/the-first"));
            Assert.AreEqual("wibble", Execute("GET", "/args/wibble"));
        }
    }
}
