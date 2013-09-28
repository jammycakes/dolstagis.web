using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.Routing
{
    public class RouteTable
    {
        private IRouteRegistry[] _modules;

        public RouteTableEntry Root { get; private set; }

        public RouteTable(params IRouteRegistry[] modules)
        {
            _modules = modules;
        }


        private void AddRouteToTable(IRouteDefinition definition)
        {
            var pathParts = definition.Route.SplitUrlPath();
            var target = Root;
            foreach (var name in pathParts) {
                target = target.GetOrCreateChild(name);
            }
            target.Definition = definition;
        }

        public void RebuildRouteTable()
        {
            Root = new RouteTableEntry(null, String.Empty);
            foreach (var module in _modules) {
                foreach (var route in module.Routes) {
                    AddRouteToTable(route);
                }
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void EnsureRouteTable()
        {
            if (Root == null) {
                RebuildRouteTable();
            }
        }

        private IEnumerable<RouteTableEntry> GetCandidates(RouteTableEntry node, string[] path, int index)
        {
            if (index >= path.Length) {
                yield return node;
            }
            else {
                foreach (var candidate in node.GetMatchingChildren(path[index])) {
                    foreach (var child in GetCandidates(candidate, path, index + 1)) {
                        yield return child;
                    }
                }
            }
        }

        private RouteInfo GetRouteInfo(RouteTableEntry entry, string[] pathParts)
        {
            var result = new RouteInfo(entry.Definition);
            int index = pathParts.Length - 1;
            while (entry != null) {
                if (entry is ParameterEntry) {
                    string key = ((ParameterEntry)entry).ParameterName;
                    result.Arguments[key] = pathParts[index];
                }
                entry = entry.Parent;
                index--;
            }
            return result;
        }

        /// <summary>
        ///  Given a path, looks up the corresponding route, and extracts its parameters.
        /// </summary>
        /// <param name="path">
        ///  The path to the route.
        /// </param>
        /// <returns>
        ///  A <see cref="RouteInfo"/> instance, or null if no routes match.
        /// </returns>

        public RouteInfo Lookup(string path)
        {
            EnsureRouteTable();
            var parts = path.SplitUrlPath();
            var candidates = GetCandidates(Root, parts, 0);
            return candidates.Select(x => GetRouteInfo(x, parts))
                .LastOrDefault(x => x.Definition != null && x.Definition.Module.Enabled);
        }
    }
}
