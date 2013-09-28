﻿using System;
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
        public void CanGetRoot()
        {
            var routeTable = new RouteTable(new FirstModule());

            var route = routeTable.Lookup("/");

            Assert.AreEqual(typeof(RootHandler), route.Definition.HandlerType);
        }
    }
}
