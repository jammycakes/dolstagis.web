using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Static;

namespace Dolstagis.Web.Views
{
    public abstract class ViewEngineBase : IViewEngine
    {
        private IDictionary<string, IView> _cache
            = new Dictionary<string, IView>(StringComparer.OrdinalIgnoreCase);
        private ISettings _settings;

        protected ViewEngineBase(ISettings settings)
        {
            _settings = settings;
        }

        public abstract IEnumerable<string> Extensions { get; }

        public IView GetView(VirtualPath pathToView, IResourceResolver resolver)
        {
            if (_settings.Debug)
            {
                return CreateView(pathToView, resolver);
            }

            IView result = null;
            string key = pathToView.Path;
            if (!_cache.TryGetValue(key, out result)) {
                result = CreateView(pathToView, resolver);
                _cache[key] = result;
            }
            return result;
        }

        protected abstract IView CreateView(VirtualPath pathToView, IResourceResolver resolver);
    }
}
