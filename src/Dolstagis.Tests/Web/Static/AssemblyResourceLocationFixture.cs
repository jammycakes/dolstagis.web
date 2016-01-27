using Dolstagis.Web;
using Dolstagis.Web.Static;
using NUnit.Framework;

namespace Dolstagis.Tests.Web.Static
{
    [TestFixture]
    public class AssemblyResourceLocationFixture
    {
        const string resourceName = "EmbeddedResource.txt";

        [Test]
        public void CanGetResourceWithinPath()
        {
            var location = new AssemblyResourceLocation(this.GetType().Assembly, this.GetType().Namespace);
            var resource = location.GetResource(new VirtualPath("~/" + resourceName));
            Assert.IsNotNull(resource);
            Assert.True(resource.Exists);
        }

        [Test]
        public void CanGetExactResource()
        {
            var location = new AssemblyResourceLocation(
                this.GetType().Assembly, this.GetType().Namespace + "." + resourceName);
            var resource = location.GetResource(new VirtualPath("~/"));
            Assert.IsNotNull(resource);
            Assert.True(resource.Exists);
        }


        [Test]
        public void NonexistentResourceReturnsNull()
        {
            var location = new AssemblyResourceLocation(this.GetType().Assembly, this.GetType().Namespace);
            var resource = location.GetResource(new VirtualPath("~/this.does.not.exist"));
            Assert.IsNull(resource);
        }
    }
}
