﻿using System;
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

        public IView GetView(VirtualPath pathToView)
        {
            IView result;
            string key = pathToView.Path;
            if (!_cache.TryGetValue(key, out result)) {
                result = CreateView(pathToView);
                _cache[key] = result;
            }
            return result;
        }

        protected abstract IView CreateView(VirtualPath pathToView);
    }
}
