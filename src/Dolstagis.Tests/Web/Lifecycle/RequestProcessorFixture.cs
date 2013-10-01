﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Tests.Web.TestModules;
using Dolstagis.Tests.Web.TestModules.Handlers;
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
    public class RequestProcessorFixture
    {
        private RouteTable _routeTable;
        private IContainer _mockContainer;

        [TestFixtureSetUp]
        public void CreateRouteTable()
        {
            _routeTable = new RouteTable(new FirstModule());
            _routeTable.RebuildRouteTable();
            var mock = new Mock<IContainer>();
            mock.Setup(x => x.GetInstance(It.IsAny<Type>())).Returns(new RootHandler());
            _mockContainer = mock.Object;
        }


        private object Execute(string method, string path)
        {
            var processor = new RequestProcessor(_routeTable, () => new ActionInvocation(_mockContainer));
            var context = new Mock<IHttpContext>();
            var request = new Mock<IHttpRequest>();
            request.SetupGet(x => x.AppRelativePath).Returns(path);
            request.SetupGet(x => x.Method).Returns(method);
            context.SetupGet(x => x.Request).Returns(request.Object);
            var task = processor.ProcessRequest(context.Object);
            task.Wait();
            return task.Result;
        }


        [Test]
        public void CanExecuteSynchronousTask()
        {
            Assert.AreEqual("Hello GET", Execute("GET", "/"));
        }

        [Test]
        public void CanExecuteAsynchronousTaskThatReturnsObject()
        {
            Assert.AreEqual("Hello POST", Execute("POST", "/"));
        }

        [Test]
        public void CanExecuteAsynchronousTaskThatReturnsString()
        {
            Assert.AreEqual("Hello PUT", Execute("PUT", "/"));
        }

        [Test]
        public void CanExecuteAsynchronousTaskThatReturnsTask()
        {
            Assert.AreEqual("Hello DELETE", Execute("DELETE", "/"));
        }
    }
}