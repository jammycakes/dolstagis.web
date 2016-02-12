using System.Collections.Generic;
using Dolstagis.Web.Static;

namespace Dolstagis.Web.Views.Nustache
{
    public class NustacheViewEngine : ViewEngineBase
    {
        private static readonly string[] _extensions = new[] { "mustache", "nustache" };

        public NustacheViewEngine(ISettings settings) : base(settings) { }

        public override IEnumerable<string> Extensions
        {
            get { return _extensions; }
        }

        protected override IView CreateView(VirtualPath pathToView, IResourceResolver resolver)
        {
            var resource = resolver.GetResource(pathToView);
            if (resource == null || !resource.Exists) {
                return null;
            }

            return new NustacheView(this, pathToView, resource, resolver);
        }
    }
}
