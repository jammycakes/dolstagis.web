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
#pragma warning disable 0618
            var registry = new ViewRegistry(new ResourceMapping[0], new[] { engine.Object });
#pragma warning restore 0618

            var gotEngine = registry.GetViewEngine(new VirtualPath("~/test.nustache"));

            Assert.AreSame(engine.Object, gotEngine);
        }

        [Test]
        public void ReturnsNullWhenViewEngineNotFound()
        {
            var engine = new Mock<IViewEngine>();
            engine.SetupGet(x => x.Extensions).Returns(new[] { "nustache" });
#pragma warning disable 0618
            var registry = new ViewRegistry(new ResourceMapping[0], new[] { engine.Object });
#pragma warning restore 0618
            Assert.IsNull(registry.GetViewEngine(new VirtualPath("~/test.cshtml")));
        }

        [TestCase("~/test.nustache", true)]
        [TestCase("~/test", true)]
        [TestCase("~/test.cshtml", false)]
        public void CanGetView(string path, bool isExpected)
        {
            var engine = new Mock<IViewEngine>();
            engine.SetupGet(x => x.Extensions).Returns(new[] { "nustache" });
#pragma warning disable 0618
            var registry = new ViewRegistry(new ResourceMapping[0], new[] { engine.Object });
#pragma warning restore 0618
            var view = Mock.Of<IView>();
#pragma warning disable 0618
            engine.Setup(x => x.GetView(new VirtualPath("~/test.nustache"), It.IsAny<ResourceResolver>()))
#pragma warning restore 0618
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
