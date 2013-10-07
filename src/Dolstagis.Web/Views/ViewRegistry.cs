using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Static;

namespace Dolstagis.Web.Views
{
    public class ViewRegistry
    {
        private IDictionary<string, IViewEngine> _viewEngines
            = new Dictionary<string, IViewEngine>(StringComparer.OrdinalIgnoreCase);

        private IDictionary<string, IView> _viewCache
            = new Dictionary<string, IView>(StringComparer.OrdinalIgnoreCase);

        private ResourceLocator _locator;

        public ViewRegistry(ResourceLocator locator, IEnumerable<IViewEngine> viewEngines)
        {
            _locator = locator;
            foreach (var engine in viewEngines)
                foreach (var ext in engine.Extensions)
                    _viewEngines[ext] = engine;
        }

        public IView GetView(VirtualPath pathToView)
        {
            if (pathToView == null) throw new ArgumentNullException("pathToView");
            if (!pathToView.Parts.Any()) throw new ArgumentException("Path to view can not be empty.", "pathToView");
            IView result;
            string key = pathToView.Path;
            if (!_viewCache.TryGetValue(key, out result)) {
                result = CreateView(pathToView);
                _viewCache[key] = result;
            }
            return result;
        }

        private IView CreateView(VirtualPath pathToView)
        {
            return null;
        }
    }
}
