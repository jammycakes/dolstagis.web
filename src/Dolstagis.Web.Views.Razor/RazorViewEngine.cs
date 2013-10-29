using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.Views.Razor
{
    public class RazorViewEngine : ViewEngineBase
    {
        private static readonly string[] _extensions = new[] { "cshtml", "vbhtml" };

        public override IEnumerable<string> Extensions
        {
            get { return _extensions; }
        }

        protected override IView CreateView(VirtualPath pathToView, Static.IResourceResolver resolver)
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
