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
            var mockRequest = new Mock<IRequestContext>();
            mockRequest.SetupGet(x => x.AppRelativePath).Returns(new VirtualPath("~/"));
            mockRequest.SetupGet(x => x.Cookies).Returns(cookies);
            var mockResponse = new Mock<IResponseContext>();
            var store = new InMemorySessionStore();
            var hcb = new HttpContextBuilder(new RouteTable(), store, () => null);

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
            var cookies = new Dictionary<string, Cookie>();
            cookies.Add(Constants.SessionKey, new Cookie(Constants.SessionKey, session.ID));

            var mockRequest = new Mock<IRequestContext>();
            mockRequest.SetupGet(x => x.AppRelativePath).Returns(new VirtualPath("~/"));
            mockRequest.SetupGet(x => x.Cookies).Returns(cookies);
            var mockResponse = new Mock<IResponseContext>();
            var hcb = new HttpContextBuilder(new RouteTable(), store, () => null);

            var ctx = hcb.CreateContext(mockRequest.Object, mockResponse.Object);
            Assert.IsNotNull(ctx.Session);

            Assert.AreSame(session, ctx.Session);
            Console.WriteLine(ctx.Session.ID);
        }
    }
}
