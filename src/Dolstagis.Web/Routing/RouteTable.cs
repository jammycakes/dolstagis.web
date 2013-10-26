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
            var pathParts = definition.Route.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            var target = Root;
            foreach (var name in pathParts) {
                target = target.GetOrCreateChild(name);
            }
            target.Definition = definition;
            // For optional parameters, back up the tree,
            // associating this definition with all optional parameters
            // and the last required one.
            while (target is ParameterEntry && ((ParameterEntry)target).Optional
                && target.Parent != null && target.Parent.Definition == null) {
                target = target.Parent;
                target.Definition = definition;
            }
        }

        public void RebuildRouteTable()
        {
            try {
                Root = new RouteTableEntry(null, String.Empty);
                foreach (var module in _modules) {
                    foreach (var route in module.Routes) {
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

        private RouteInfo GetRouteInfo(RouteTableEntry entry, string[] pathParts)
        {
            var e = entry;
            var stack = new Stack<RouteTableEntry>();
            while (e.Parent != null) { // ignore the root element
                stack.Push(e);
                e = e.Parent;
            }

            var result = new RouteInfo(entry.Definition);
            int index = 0;
            while (stack.Count > 0 && index < pathParts.Length) {
                e = stack.Pop();
                if (e is ParameterEntry) {
                    var pe = (ParameterEntry)e;
                    string key = pe.ParameterName;
                    if (pe.Greedy) {
                        result.Arguments[key] = String.Join("/", pathParts.Skip(index).ToArray());
                    }
                    else {
                        result.Arguments[key] = pathParts[index];
                    }
                }
                index++;
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

        public IEnumerable<RouteInfo> Lookup(VirtualPath path)
        {
            EnsureRouteTable();
            var parts = path.Parts.ToArray();
            var candidates = GetCandidates(Root, parts, 0);
            return candidates.Select(x => GetRouteInfo(x, parts))
                .Where(x => x.Definition != null && x.Definition.Module.Enabled);
        }
    }
}
