using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using global::Nustache.Core;

namespace Dolstagis.Web.Views.Nustache
{
    public class NustacheViewEngine : ViewEngineBase
    {
        private static readonly string[] _extensions = new[] { "nustache" };

        public override IEnumerable<string> Extensions
        {
            get { return _extensions; }
        }

        protected override IView CreateView(VirtualPath pathToView, Static.IResourceLocator locator)
        {
            throw new NotImplementedException();
        }
    }
}
