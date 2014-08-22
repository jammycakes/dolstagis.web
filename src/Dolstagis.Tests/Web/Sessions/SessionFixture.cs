using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web;
using Dolstagis.Web.Http;
using Dolstagis.Web.Lifecycle;
using Dolstagis.Web.Routing;
using Dolstagis.Web.Sessions;
using Moq;
using NUnit.Framework;

namespace Dolstagis.Tests.Web.Sessions
{
    [TestFixture]
    public class SessionFixture
    {

        [Test]
        public void CanGetNewSession()
        {
            var cookies = new Dictionary<string, Cookie>();
            var mockRequest = new Mock<IRequest>();
            var headers = new RequestHeaders(new Dictionary<string, string[]>());
            mockRequest.SetupGet(x => x.Path).Returns(new VirtualPath("~/"));
            mockRequest.SetupGet(x => x.Headers).Returns(headers);
            var mockResponse = new Mock<IResponse>();
            var store = new InMemorySessionStore();
            var hcb = new RequestContextBuilder(new RouteTable(), store, null, () => null);

            var ctx = hcb.CreateContext(mockRequest.Object, mockResponse.Object);
            Assert.IsNotNull(ctx.Session);

            var session = store.GetSession(ctx.Session.ID);
            Assert.AreSame(session, ctx.Session);
            Console.WriteLine(ctx.Session.ID);
        }


        [Test]
        public void CanGetSessionFromCookie()
        {
            var store = new InMemorySessionStore();
            var session = store.GetSession(null);

            var cookie = new Cookie(Constants.SessionKey, session.ID);

            var headers = new RequestHeaders(new Dictionary<string, string[]>());
            headers["Cookie"] = new string[] { cookie.ToHeaderString() };

            var mockRequest = new Mock<IRequest>();
            mockRequest.SetupGet(x => x.Path).Returns(new VirtualPath("~/"));
            mockRequest.SetupGet(x => x.Headers).Returns(headers);
            var mockResponse = new Mock<IResponse>();
            var hcb = new RequestContextBuilder(new RouteTable(), store, null, () => null);

            var ctx = hcb.CreateContext(mockRequest.Object, mockResponse.Object);
            Assert.IsNotNull(ctx.Session);

            Assert.AreSame(session, ctx.Session);
            Console.WriteLine(ctx.Session.ID);
        }
    }
}
