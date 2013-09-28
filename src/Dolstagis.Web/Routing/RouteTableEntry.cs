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

        public bool IsParameter { get { return this is ParameterEntry; } }

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
