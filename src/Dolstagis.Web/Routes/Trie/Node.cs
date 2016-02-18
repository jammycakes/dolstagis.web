using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.Routes.Trie
{
    public class Node<TNode, TItem> where TNode : Node<TNode, TItem>, new()
    {
        private IDictionary<string, TNode> _children
            = new Dictionary<string, TNode>(StringComparer.OrdinalIgnoreCase);
        private IList<TNode> _parameters = new List<TNode>();

        public IList<TItem> Items { get; } = new List<TItem>();

        public string Name { get; private set; }

        public TNode Parent { get; private set; }

        public int Level { get; private set; } = 0;

        public TNode GetOrCreateChild(string name)
        {
            TNode child;
            if (!_children.TryGetValue(name, out child)) {
                child = new TNode() {
                    Parent = (TNode)this,
                    Name = name,
                    Level = this.Level + 1
                };
                child.Validate();
                _children.Add(name, child);
                if (child.IsParameter) {
                    _parameters.Add(child);
                }
            }
            return child;
        }

        public virtual bool IsParameter
        {
            get { return false; }
        }

        public virtual bool Optional
        {
            get { return false; }
        }

        public virtual bool Greedy
        {
            get { return false; }
        }

        public virtual string ParameterName
        {
            get { return null; }
        }

        protected virtual void Validate()
        { }

        private IEnumerable<TNode> GetOptionalChildren()
        {
            foreach (var node in this._parameters.Where(x => x.Optional)) {
                foreach (var found in node.GetOptionalChildren()) {
                    yield return found;
                }
                yield return node;
            }
        }

        public IEnumerable<TNode> Find(VirtualPath path)
        {
            /*
             * If it's greedy, it matches. Period.
             * Greedy parameters don't have children.
             */

            if (Greedy) {
                yield return (TNode)this;
                yield break;
            }

            /*
             * If it's the last level, it matches, but so do all its optional
             * children.
             */

            var lastLevel = path.Parts.Count - 1;
            if (Level > lastLevel) {
                foreach (var child in GetOptionalChildren()) {
                    yield return child;
                }
                yield return (TNode)this;
                yield break;
            }

            // Otherwise think of the children
            else {
                // Look for an exact match
                TNode node;
                if (_children.TryGetValue(path.Parts[Level], out node) && !node.IsParameter) {
                    foreach (var childNode in node.Find(path)) {
                        yield return childNode;
                    }
                }

                // And for a parameter match
                foreach (var param in _parameters.SelectMany(x => x.Find(path)))
                    yield return param;
            }
        }

        public virtual string ExtractArgument(VirtualPath path)
        {
            /*
             * Level 0 is the root node. This doesn't match anything.
             * Level 1 matches on the first item in the parts array i.e. Parts[0].
             */

            if (Level > path.Parts.Count)
                return null;
            else if (Greedy)
                return String.Join("/", path.Parts.Skip(Level - 1));
            else
                return path.Parts[Level - 1];
        }
    }
}
