using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Static;

namespace Dolstagis.Web.Views.DotLiquid
{
    public class DotLiquidViewEngine : ViewEngineBase
    {
        public DotLiquidViewEngine(ISettings settings) : base(settings) { }

        public override IEnumerable<string> Extensions
        {
            get {
                return new[] { "liquid" };
            }
        }

        protected override IView CreateView(VirtualPath pathToView, ResourceResolver resolver)
        {
            var resource = resolver.GetResource(pathToView);
            if (resource == null || !resource.Exists) {
                return null;
            }

            return new DotLiquidView(this, pathToView, resource, resolver);
        }
    }
}
