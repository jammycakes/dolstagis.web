using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.Routing
{
    public static class RoutingHelpers
    {
        private static string[] NormalisePathParts(string[] path)
        {
            var parts = new Stack<string>();
            foreach (var part in path) {
                if (part == "..") {
                    if (parts.Count > 0) {
                        parts.Pop();
                    }
                }
                else if (!String.IsNullOrEmpty(part)) {
                    parts.Push(part);
                }
            }
            return parts.Reverse().ToArray();
        }

        /// <summary>
        ///  Splits a URL path into its component parts.
        /// </summary>
        /// <param name="path">
        ///  The path to split.
        /// </param>
        /// <returns>
        ///  An array of path components.
        /// </returns>

        public static string[] SplitUrlPath(this string path)
        {
            return NormalisePathParts(path.Split('/'));
        }

        /// <summary>
        ///  Splits a URL path into its component parts.
        /// </summary>
        /// <param name="path">
        ///  The path to split.
        /// </param>
        /// <param name="count">
        ///  The maximum number of components to return.
        ///  If zero, 
        /// </param>
        /// <returns>
        ///  An array of path components.
        /// </returns>

        public static string[] SplitUrlPath(this string path, int parts)
        {
            return NormalisePathParts(path.Split(new char[] { '/' }, parts));
        }

        /// <summary>
        ///  Normalises a URL path
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>The normalised path.</returns>

        public static string NormaliseUrlPath(this string path)
        {
            return String.Join("/", SplitUrlPath(path));
        }
    }
}
