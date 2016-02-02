﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dolstagis.Web;
using Dolstagis.Web.Features;
using Dolstagis.Web.Features.Impl;
using Dolstagis.Web.Http;
using Dolstagis.Web.Lifecycle;
using Dolstagis.Web.Sessions;
using Moq;
using NUnit.Framework;

namespace Dolstagis.Tests.Web.Sessions
{
    [TestFixture]
    public class SessionFixture
    {

        [Test]
        public async Task CanGetNewSession()
        {
            var cookies = new Dictionary<string, Cookie>();
            var mockRequest = new Mock<IRequest>();
            var headers = new RequestHeaders(new Dictionary<string, string[]>());
            mockRequest.SetupGet(x => x.Path).Returns(new VirtualPath("~/"));
            mockRequest.SetupGet(x => x.Headers).Returns(headers);
            var mockResponse = new Mock<IResponse>();
            var store = new InMemorySessionStore();

            var processor = new RequestProcessor
                (null, null, store, null, new FeatureSet(null, new IFeature[0]), null);

            var ctx = processor.CreateContext(mockRequest.Object, mockResponse.Object);
            Assert.IsNotNull(ctx.Session);

            var session = await store.GetSession(ctx.Session.ID);
            Assert.AreSame(session, ctx.Session);
            Console.WriteLine(ctx.Session.ID);
        }


        [Test]
        public async Task CanGetSessionFromCookie()
        {
            var store = new InMemorySessionStore();
            var session = await store.GetSession(null);

            var cookie = new Cookie(Constants.SessionKey, session.ID);

            var headers = new RequestHeaders(new Dictionary<string, string[]>());
            headers["Cookie"] = new string[] { cookie.ToHeaderString() };

            var mockRequest = new Mock<IRequest>();
            mockRequest.SetupGet(x => x.Path).Returns(new VirtualPath("~/"));
            mockRequest.SetupGet(x => x.Headers).Returns(headers);
            var mockResponse = new Mock<IResponse>();
            var processor = new RequestProcessor
                (null, null, store, null, new FeatureSet(null, new IFeature[0]), null);

            var ctx = processor.CreateContext(mockRequest.Object, mockResponse.Object);
            Assert.IsNotNull(ctx.Session);

            Assert.AreSame(session, ctx.Session);
            Console.WriteLine(ctx.Session.ID);
        }
    }
}
