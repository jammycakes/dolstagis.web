using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Routing;

namespace Dolstagis.Web.Views
{
    public abstract class ViewEngineBase : IViewEngine
    {
        private IDictionary<string, IView> _cache
            = new Dictionary<string, IView>(StringComparer.OrdinalIgnoreCase);

        public abstract IEnumerable<string> Extensions { get; }

        public IView GetView(string pathToView)
        {
            IView result;
            string key = pathToView.NormaliseUrlPath();
            if (!_cache.TryGetValue(key, out result)) {
                result = CreateView(key);
                _cache[key] = result;
            }
            return result;
        }

        protected abstract IView CreateView(string pathToView);
    }
}
