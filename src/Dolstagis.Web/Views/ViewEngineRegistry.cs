using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Static;

namespace Dolstagis.Web.Views
{
    public class ViewEngineRegistry
    {
        private IDictionary<string, IViewEngine> _viewEngines
            = new Dictionary<string, IViewEngine>(StringComparer.OrdinalIgnoreCase);

        private ResourceLocator _locator;

        public ViewEngineRegistry(ResourceLocator locator, IEnumerable<IViewEngine> viewEngines)
        {
            _locator = locator;
            foreach (var engine in viewEngines)
                foreach (var ext in engine.Extensions)
                    _viewEngines[ext] = engine;
        }
    }
}
