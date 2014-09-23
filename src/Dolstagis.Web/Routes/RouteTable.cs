using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Http;

namespace Dolstagis.Web.Routes
{
    /// <summary>
    ///  A routing table.
    /// </summary>

    public class RouteTable : IRouteTable
    {
        public Node Root { get; private set; }


        public RouteTable()
        {
            this.Root = new Node(String.Empty);
        }

        /// <summary>
        ///  Registers a route on this route table.
        /// </summary>
        /// <param name="specification">
        ///  The route specification. null or an empty string indicates the root
        ///  node.
        /// </param>
        /// <param name="target">
        ///  The route target.
        /// </param>
        /// <param name="constraint">
        ///  A predicate which allows us to add constraints to the route.
        /// </param>

        public void Add(string specification, IRouteTarget target)
        {
            var pathParts = specification.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            var node = Root;
            foreach (var name in pathParts)
            {
                node = node.GetOrCreateChild(name);
            }

            node.Targets.Add(target);

            // When an optional parameter is missing, the route invocation will
            // land on the parent node, which will not otherwise be assigned to
            // this route. Therefore in this case, back up the tree associating
            // this definition with all optional parameters and the last
            // required one.
            while (node is Parameter && ((Parameter)node).Optional
                && node.Parent != null)
            {
                node = node.Parent;
                node.Targets.Add(target);
            }
        }


        private IEnumerable<Node> GetCandidates(Node node, string[] path, int index)
        {
            if (index >= path.Length || (node is Parameter && ((Parameter)node).Greedy))
            {
                yield return node;
            }
            else
            {
                foreach (var candidate in node.GetMatchingChildren(path[index]))
                {
                    foreach (var child in GetCandidates(candidate, path, index + 1))
                    {
                        yield return child;
                    }
                }
            }
        }


        /// <summary>
        ///  Gets the route invocation for the requested path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>

        public RouteInvocation GetRouteInvocation(VirtualPath path)
        {
            var parts = path.Parts.ToArray();
            var candidates = GetCandidates(Root, parts, 0);

            var cts =
                from candidate in candidates
                from target in candidate.Targets
                select new { candidate = candidate, target = target };

            var ct = cts.LastOrDefault();
            if (ct == null) return null;
            return new RouteInvocation(ct.target, GetRouteArguments(ct.candidate, parts));
        }

        private IDictionary<string, string> GetRouteArguments(Node candidate, string[] parts)
        {
            // First get the candidate's ancestry, oldest first.
            var node = candidate;
            var stack = new Stack<Node>();
            while (node.Parent != null)
            {
                // ignore the root element, it doesn't match any part of the path
                stack.Push(node);
                node = node.Parent;
            }

            var arguments = new Dictionary<string, string>();

            int index = 0;
            while (stack.Count > 0 && index < parts.Length)
            {
                node = stack.Pop();
                if (node is Parameter)
                {
                    var parameter = (Parameter)node;
                    string key = parameter.ParameterName;
                    if (parameter.Greedy)
                    {
                        arguments[key] = String.Join("/", parts.Skip(index).ToArray());
                    }
                    else
                    {
                        arguments[key] = parts[index];
                    }
                }
                index++;
            }

            return arguments;
        }
    }
}
