using System;
using System.Collections.Generic;
using System.Linq;

namespace Dolstagis.Web.Routes
{
    public class Node
    {
        private IDictionary<string, Node> _children
            = new Dictionary<string, Node>(StringComparer.OrdinalIgnoreCase);

        private IList<Parameter> _parameterChildren = new List<Parameter>();

        public IList<IRouteTarget> Targets { get; set; }

        public string Name { get; private set; }

        public Node Parent { get; private set; }

        public Node(string name)
        {
            this.Name = name;
            this.Targets = new List<IRouteTarget>();
        }

        public virtual Node GetOrCreateChild(string name)
        {
            bool isParameter = name.StartsWith("{") && name.EndsWith("}");
            Node child;
            if (!_children.TryGetValue(name, out child)) {
                child = isParameter
                    ? new Parameter(name)
                    : new Node(name);
                child.Parent = this;
                _children.Add(name, child);
                if (child is Parameter) {
                    _parameterChildren.Add((Parameter)child);
                }
            }
            return child;
        }

        public IEnumerable<Node> GetMatchingChildren(string name)
        {
            Node child;
            if (_children.TryGetValue(name, out child) && !(child is Parameter)) {
                return Enumerable.Repeat(child, 1);
            }
            else {
                return _parameterChildren;
            }
        }
    }
}
