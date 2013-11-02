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
            var resolver = new Mock<IResourceResolver>();
            var registry = new ViewRegistry(resolver.Object, new[] { engine.Object });

            var gotEngine = registry.GetViewEngine(new VirtualPath("~/test.nustache"));

            Assert.AreSame(engine.Object, gotEngine);
        }

        [Test]
        public void ReturnsNullWhenViewEngineNotFound()
        {
            var engine = new Mock<IViewEngine>();
            engine.SetupGet(x => x.Extensions).Returns(new[] { "nustache" });
            var resolver = new Mock<IResourceResolver>();
            var registry = new ViewRegistry(resolver.Object, new[] { engine.Object });
            Assert.IsNull(registry.GetViewEngine(new VirtualPath("~/test.cshtml")));
        }

        [TestCase("~/test.nustache", true)]
        [TestCase("~/test", true)]
        [TestCase("~/test.cshtml", false)]
        public void CanGetView(string path, bool isExpected)
        {
            var engine = new Mock<IViewEngine>();
            engine.SetupGet(x => x.Extensions).Returns(new[] { "nustache" });
            var resolver = new Mock<IResourceResolver>();
            var registry = new ViewRegistry(resolver.Object, new[] { engine.Object });
            var view = Mock.Of<IView>();
            engine.Setup(x => x.GetView(new VirtualPath("~/test.nustache"), It.IsAny<IResourceResolver>()))
                .Returns(view);
            var gotView = registry.GetView(new VirtualPath(path));
            if (isExpected)
            {
                Assert.AreSame(view, gotView);
            }
            else
            {
                Assert.IsNull(gotView);
            }
        }
    }
}
