using System.Collections.Generic;
using Dolstagis.Web.Static;
using DotLiquid;
using DotLiquid.FileSystems;

namespace Dolstagis.Web.Views.DotLiquid
{
    public class DotLiquidViewEngine : ViewEngineBase
    {
        public DotLiquidViewEngine(ISettings settings, IFileSystem fileSystem) : base(settings) {
            Template.FileSystem = fileSystem;
        }

        public override IEnumerable<string> Extensions
        {
            get {
                return new[] { "liquid" };
            }
        }

        protected override IView CreateView(VirtualPath pathToView, IResourceResolver resolver)
        {
            var resource = resolver.GetResource(pathToView);
            if (resource == null || !resource.Exists) {
                return null;
            }

            return new DotLiquidView(pathToView, resource, resolver);
        }
    }
}
