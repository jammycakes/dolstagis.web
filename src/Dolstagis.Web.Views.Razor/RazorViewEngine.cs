using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Razor;
using Dolstagis.Web.Static;

namespace Dolstagis.Web.Views.Razor
{
    public class RazorViewEngine : ViewEngineBase
    {
        private readonly string _extension;
        private readonly RazorCodeLanguage _language;

        public RazorViewEngine(string extension, RazorCodeLanguage language)
        {
            _extension = extension;
            _language = language;
        }

        public override IEnumerable<string> Extensions
        {
            get { return new[] { _extension }; }
        }

        protected override IView CreateView(VirtualPath pathToView, IResourceResolver resolver)
        {
            var resource = resolver.GetResource(pathToView);
            if (resource == null || !resource.Exists)
            {
                throw new ViewNotFoundException("There is no view at " + pathToView.ToString());
            }

            return new RazorView(this, pathToView, resource, resolver);
        }
    }
}
