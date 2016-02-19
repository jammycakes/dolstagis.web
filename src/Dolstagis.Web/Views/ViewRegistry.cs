using System;
using System.Collections.Generic;
using System.Linq;
using Dolstagis.Web.Features;
using Dolstagis.Web.Static;

namespace Dolstagis.Web.Views
{
    /// <summary>
    ///  The ViewRegistry is the class that aggregates all the view tables from
    ///  the active features.
    /// </summary>
    /// <remarks>
    ///  The ViewRegistry is registered in singleton scope to the feature
    ///  container so we don't have to re-create it every time. It will then be
    ///  injected into the request-scoped ViewResover instances as needed.
    /// </remarks>

    public class ViewRegistry
    {
        private IDictionary<string, IViewEngine> _viewEngines
            = new Dictionary<string, IViewEngine>(StringComparer.OrdinalIgnoreCase);

        private IList<ViewTable> _viewTables = new List<ViewTable>();

        public ViewRegistry(IEnumerable<IFeature> features, IEnumerable<IViewEngine> viewEngines)
        {
            foreach (var engine in viewEngines)
                foreach (var ext in engine.Extensions)
                    _viewEngines[ext] = engine;

            /*
             * Switchable features take priority over non-switchable features
             * so include these first.
             */

            var orderedFeatures =
                from feature in features
                let x = feature.Switch.IsDefined ? 0 : 1
                orderby x
                select feature;

            foreach (var feature in orderedFeatures) {
                _viewTables.Add(feature.Views);
            }
        }


        /// <summary>
        ///  Gets the <see cref="ViewInfo"/> instance for the first available
        ///  view: the view registration and the relative path.
        /// </summary>
        /// <param name="pathToView"></param>
        /// <returns></returns>

        public ViewInfo GetViewInfo(VirtualPath pathToView)
        {
            var registration =
                from table in _viewTables
                let matches = table.GetMatches(pathToView)
                from match in matches
                let viewPath = match.Parameters["path"].FirstOrDefault()
                where viewPath != null
                from item in match.Node.Items
                where item != null
                select new ViewInfo() {
                    Location = item.Location,
                    RelativePath = viewPath
                };

            return registration.FirstOrDefault();
        }


        /// <summary>
        ///  Gets a list of registered extensions for this view.
        /// </summary>
        /// <returns></returns>

        public IEnumerable<string> GetRegisteredExtensions()
        {
            return _viewEngines.Keys;
        }


        public IViewEngine GetViewEngine(string extension)
        {
            IViewEngine result;
            return _viewEngines.TryGetValue(extension, out result)
                ? result : null;
        }
    }
}