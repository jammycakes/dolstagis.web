using System;
using System.Collections.Generic;
using System.Linq;
using Dolstagis.Web.Static;

namespace Dolstagis.Web.Views
{
    public class ViewRegistry
    {
        private IDictionary<string, IViewEngine> _viewEngines
            = new Dictionary<string, IViewEngine>(StringComparer.OrdinalIgnoreCase);

        private IResourceResolver _resolver;

        public ViewRegistry(ResourceMapping[] mappings, IViewEngine[] viewEngines)
        {
            _resolver = new ResourceResolver(mappings);
            foreach (var engine in viewEngines)
                foreach (var ext in engine.Extensions)
                    _viewEngines[ext] = engine;
        }

        /// <summary>
        ///  Gets the view engine which will handle this view.
        /// </summary>
        /// <param name="pathToView"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        ///  pathToView is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///  No extension is specified for the view.
        /// </exception>
        public IViewEngine GetViewEngine(VirtualPath pathToView)
        {
            if (pathToView == null) {
                throw new ArgumentNullException("pathToView");
            }
            if (pathToView.Parts.Count == 0) return null;
            var split = pathToView.Parts.Last().Split('.');
            if (split.Length <= 1) return null;

            IViewEngine result;
            if (_viewEngines.TryGetValue(split.Last(), out result)) return result;
            return null;
        }

        /// <summary>
        ///  Gets a view.
        /// </summary>
        /// <param name="pathToView"></param>
        /// <returns></returns>
        public IView GetView(VirtualPath pathToView)
        {
            var extendedPaths =
                from extension in _viewEngines.Keys
                select new VirtualPath(pathToView.ToString() + "." + extension);
            var allPaths = new[] { pathToView }.Concat(extendedPaths);
            var views =
                from path in allPaths
                let engine = this.GetViewEngine(path)
                where engine != null
                let view = engine.GetView(path, _resolver)
                where view != null
                select view;
            return views.FirstOrDefault();
        }
    }
}
