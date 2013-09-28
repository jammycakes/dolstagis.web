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

        private IList<RouteTableEntry> _parameterChildren
            = new List<RouteTableEntry>();

        public IRouteDefinition Definition { get; set; }

        public string Name { get; private set; }

        public bool IsParameter { get; private set; }

        public RouteTableEntry(IRouteDefinition definition, string name)
        {
            this.Definition = definition;
            this.Name = name;
            this.IsParameter = name.StartsWith("{") && name.EndsWith("}");
        }

        public RouteTableEntry GetOrCreateChild(string name)
        {
            RouteTableEntry child;
            if (!_children.TryGetValue(name, out child)) {
                child = new RouteTableEntry(null, name);
                _children.Add(name, child);
                if (child.IsParameter) {
                    _parameterChildren.Add(child);
                }
            }
            return child;
        }

        public IEnumerable<RouteTableEntry> GetMatchingChildren(string name)
        {
            RouteTableEntry child;
            if (_children.TryGetValue(name, out child) && !child.IsParameter) {
                return Enumerable.Repeat(child, 1);
            }
            else {
                return _parameterChildren;
            }
        }
    }
}
