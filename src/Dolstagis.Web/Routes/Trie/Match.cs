using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.Routes.Trie
{
    public class Match<TNode, TItem> where TNode : Node<TNode, TItem>, new()
    {
        public TNode Node { get; private set; }

        public VirtualPath Path { get; private set; }

        public IDictionary<string, IList<string>> Parameters { get; private set; }
            = new Dictionary<string, IList<string>>();

        public Match(TNode node, VirtualPath path)
        {
            Node = node;
            Path = path;

            TNode currentNode = node;
            while (currentNode != null) {
                if (currentNode.IsParameter) {
                    IList<string> values;
                    if (!Parameters.TryGetValue(currentNode.ParameterName, out values)) {
                        values = new List<string>();
                        Parameters[currentNode.ParameterName] = values;
                    }
                    values.Insert(0, currentNode.ExtractArgument(path));
                }
                currentNode = currentNode.Parent;
            }
        }
    }
}
