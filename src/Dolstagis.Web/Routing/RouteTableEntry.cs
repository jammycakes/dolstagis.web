using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.Routing
{
    public class RouteTableEntry
    {
        private IDictionary<string, RouteTableEntry> _children
            = new Dictionary<string, RouteTableEntry>(StringComparer.OrdinalIgnoreCase);

        private IList<ParameterEntry> _parameterChildren
            = new List<ParameterEntry>();

        public IRouteDefinition Definition { get; set; }

        public string Name { get; private set; }

        public RouteTableEntry Parent { get; private set; }

        public RouteTableEntry(IRouteDefinition definition, string name)
        {
            this.Definition = definition;
            this.Name = name;
        }

        public virtual RouteTableEntry GetOrCreateChild(string name)
        {
            bool isParameter = name.StartsWith("{") && name.EndsWith("}");
            RouteTableEntry child;
            if (!_children.TryGetValue(name, out child)) {
                child = isParameter
                    ? new ParameterEntry(null, name)
                    : new RouteTableEntry(null, name);
                child.Parent = this;
                _children.Add(name, child);
                if (child is ParameterEntry) {
                    _parameterChildren.Add((ParameterEntry)child);
                }
            }
            return child;
        }

        public IEnumerable<RouteTableEntry> GetMatchingChildren(string name)
        {
            RouteTableEntry child;
            if (_children.TryGetValue(name, out child) && !(child is ParameterEntry)) {
                return Enumerable.Repeat(child, 1);
            }
            else {
                return _parameterChildren;
            }
        }
    }
}
