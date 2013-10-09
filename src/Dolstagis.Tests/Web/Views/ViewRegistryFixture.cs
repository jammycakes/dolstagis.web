using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web;
using Dolstagis.Web.Static;
using Dolstagis.Web.Views;
using Moq;
using NUnit.Framework;

namespace Dolstagis.Tests.Web.Views
{
    [TestFixture]
    public class ViewRegistryFixture
    {
        [Test]
        public void CanGetViewEngine()
        {
            var engine = new Mock<IViewEngine>();
            engine.SetupGet(x => x.Extensions).Returns(new[] { "nustache" });
            var locator = new Mock<IResourceLocator>();
            var registry = new ViewRegistry(locator.Object, new[] { engine.Object });

            var gotEngine = registry.GetViewEngine(new VirtualPath("~/test.nustache"));

            Assert.AreSame(engine.Object, gotEngine);
        }

        [Test]
        [ExpectedException(typeof(ViewEngineNotFoundException))]
        public void CanNotGetViewEngineWhenNotFound()
        {
            var engine = new Mock<IViewEngine>();
            engine.SetupGet(x => x.Extensions).Returns(new[] { "nustache" });
            var locator = new Mock<IResourceLocator>();
            var registry = new ViewRegistry(locator.Object, new[] { engine.Object });
            registry.GetViewEngine(new VirtualPath("~/test.cshtml"));
        }
    }
}
