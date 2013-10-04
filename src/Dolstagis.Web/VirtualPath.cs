using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web
{
    /// <summary>
    ///  Rw
    /// </summary>

    public class VirtualPath
    {
        public VirtualPathType Type { get; private set; }

        public string Path { get; private set; }

        public IList<string> Parts { get; private set; }

        public VirtualPath(string path)
        {
            if (path.StartsWith("/")) {
                Type = VirtualPathType.Absolute;
                path = path.Substring(1);
            }
            else if (path.StartsWith("~/")) {
                Type = VirtualPathType.AppRelative;
                path = path.Substring(2);
            }
            else {
                Type = VirtualPathType.RequestRelative;
            }

            Parts = GetParts(path).ToList().AsReadOnly();
            Path = String.Join("/", Parts);
        }

        public VirtualPath(IEnumerable<string> parts, VirtualPathType type)
        {
            Type = type;
            Parts = GetParts(parts).ToList().AsReadOnly();
            Path = String.Join("/", Parts);
        }

        private IEnumerable<string> GetParts(string path)
        {
            return GetParts(path.Trim('/').Split('/'));
        }

        private IEnumerable<string> GetParts(IEnumerable<string> parts)
        {
            var stack = new Stack<string>();

            foreach (var part in parts) {
                if (stack.Count == 0) {
                    stack.Push(part);
                }
                else if (part == ".." && stack.Peek() != "..") {
                    stack.Pop();
                }
                else {
                    stack.Push(part);
                }
            }
            var result = stack.Reverse();
            if (Type == VirtualPathType.Absolute) {
                result = result.SkipWhile(x => x == "..");
            }
            return result;
        }

        public VirtualPath Append(VirtualPath other)
        {
            if (other.Type != VirtualPathType.RequestRelative) {
                return other;
            }
            else {
                return new VirtualPath(this.Parts.Concat(other.Parts), this.Type);
            }
        }
    }
}
