using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.Routes.Trie
{
    public class Trie<TNode, TItem> where TNode : Node<TNode, TItem>, new()
    {
        private TNode _root = new TNode();

        public void Add(VirtualPath specification, TItem item)
        {
            var node = _root;
            foreach (var part in specification.Parts) {
                node = node.GetOrCreateChild(part);
            }
            node.Items.Add(item);
        }

        public IEnumerable<TNode> Find(VirtualPath path)
        {
            return
                from result in _root.Find(path)
                orderby result.Level descending, result.IsParameter ? 0 : 1
                select result;
        }

        public IEnumerable<Match<TNode, TItem>> GetMatches(VirtualPath path)
        {
            return Find(path).Select(node => new Match<TNode, TItem>(node, path));
        }
    }
}
