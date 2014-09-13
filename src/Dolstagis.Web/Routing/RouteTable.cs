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
        private IRouteRegistry[] _features;

        public RouteTableEntry Root { get; private set; }

        public RouteTable(params IRouteRegistry[] features)
        {
            _features = features;
        }


        private void AddRouteToTable(IRouteDefinition definition)
        {
            var pathParts = definition.Route.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            var target = Root;
            foreach (var name in pathParts) {
                target = target.GetOrCreateChild(name);
            }
            target.Definitions.Add(definition);
            // For optional parameters, back up the tree,
            // associating this definition with all optional parameters
            // and the last required one.
            while (target is ParameterEntry && ((ParameterEntry)target).Optional
                && target.Parent != null) {
                target = target.Parent;
                target.Definitions.Add(definition);
            }
        }

        public void RebuildRouteTable()
        {
            try {
                Root = new RouteTableEntry(String.Empty);
                foreach (var feature in _features) {
                    foreach (var route in feature.LegacyRoutes) {
                        AddRouteToTable(route);
                    }
                }
            }
            catch {
                Root = null;
                throw;
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
            if (index >= path.Length || (node is ParameterEntry && ((ParameterEntry)node).Greedy)) {
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

        private IEnumerable<RouteInfo> GetRouteInfo(RouteTableEntry entry, string[] pathParts)
        {
            var e = entry;
            var stack = new Stack<RouteTableEntry>();
            while (e.Parent != null) { // ignore the root element
                stack.Push(e);
                e = e.Parent;
            }

            var arguments = new Dictionary<string, string>();

            int index = 0;
            while (stack.Count > 0 && index < pathParts.Length) {
                e = stack.Pop();
                if (e is ParameterEntry) {
                    var pe = (ParameterEntry)e;
                    string key = pe.ParameterName;
                    if (pe.Greedy) {
                        arguments[key] = String.Join("/", pathParts.Skip(index).ToArray());
                    }
                    else {
                        arguments[key] = pathParts[index];
                    }
                }
                index++;
            }

            return entry.Definitions.Select(definition => new RouteInfo(definition, arguments));
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

        public IEnumerable<RouteInfo> Lookup(VirtualPath path)
        {
            EnsureRouteTable();
            var parts = path.Parts.ToArray();
            var candidates = GetCandidates(Root, parts, 0);
            return
                from candidate in candidates
                from routeInfo in GetRouteInfo(candidate, parts)
                where routeInfo.Definition != null
                select routeInfo;
        }
    }
}
