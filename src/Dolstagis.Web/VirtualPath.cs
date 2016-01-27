using System;
using System.Collections.Generic;
using System.Linq;

namespace Dolstagis.Web
{
    /// <summary>
    ///  Represents a virtual path within the URL space. This can be either
    ///  absolute, relative to the application, or relative to another path.
    /// </summary>

    public class VirtualPath
    {
        /// <summary>
        ///  Indicates the type of path, whether it is absolute or relative.
        /// </summary>

        public VirtualPathType Type { get; private set; }

        /// <summary>
        ///  Gets a string representation of the path. Regardless of the type,
        ///  this will not have a leading or trailing slash.
        /// </summary>

        public string Path { get; private set; }

        /// <summary>
        ///  Gets a decomposition of the path into its constituent parts.
        /// </summary>

        public IList<string> Parts { get; private set; }

        /// <summary>
        ///  Creates a new instance of the <see cref="VirtualPath"/> class,
        ///  from the provided path.
        /// </summary>
        /// <param name="path">
        ///  The path.
        /// </param>

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

        /// <summary>
        ///  Creates a new instance of the <see cref="VirtualPath"/> class,
        ///  from the constituent parts of the path and its type.
        /// </summary>
        /// <param name="parts"></param>
        /// <param name="type"></param>

        public VirtualPath(IEnumerable<string> parts, VirtualPathType type)
        {
            Type = type;
            Parts = GetParts(parts).ToList().AsReadOnly();
            Path = String.Join("/", Parts);
        }

        private IEnumerable<string> GetParts(string path)
        {
            var parts = path.Trim('/').Split('/');
            if (parts.Length == 1 && String.IsNullOrEmpty(parts[0])) {
                parts = new string[0];
            }
            return GetParts(parts);
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

        /// <summary>
        ///  Appends one virtual path to another.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>

        public VirtualPath Append(VirtualPath other)
        {
            if (other.Type == VirtualPathType.Absolute || 
                (other.Type == VirtualPathType.AppRelative && this.Type != VirtualPathType.Absolute)) {
                return other;
            }
            else {
                return new VirtualPath(this.Parts.Concat(other.Parts), this.Type);
            }
        }

        /// <summary>
        ///  Given a path that is a sub-path of this path, returns the difference
        ///  between the two of them.
        /// </summary>
        /// <param name="other">
        ///  The longer of the two paths.
        /// </param>
        /// <param name="ignoreCase">
        ///  true to perform a case-insensitive comparison, or false for case-sensitive.
        /// </param>
        /// <returns>
        ///  A request-relative virtual path from this path to other. Returns null if
        ///    (a) both paths are differnt types
        ///    (b) both paths are request-relative, or
        ///    (c) other is not a sub-path of this.
        /// </returns>

        public VirtualPath GetSubPath(VirtualPath other, bool ignoreCase)
        {
            if (other == null) return null;
            if (other.Type != this.Type) return null;
            if (this.Type == VirtualPathType.RequestRelative) return null;
            return GetSubPathInternal(other, ignoreCase, VirtualPathType.RequestRelative);
        }

        /// <summary>
        ///  Given that this path is the application path, and that the other path is the
        ///  request path, gets the app-relative path.
        /// </summary>
        /// <param name="other">
        ///  The request path.
        /// </param>
        /// <param name="ignoreCase">
        ///  true to perform a case-insensitive comparison, or false for case-sensitive.
        /// </param>
        /// <returns>
        ///  A request-relative virtual path from this path to other. Returns null if
        ///    (a) either path is not absolute, or
        ///    (c) other is not a sub-path of this.
        /// </returns>

        public VirtualPath GetAppRelativePath(VirtualPath other, bool ignoreCase)
        {
            if (other == null) return null;
            if (this.Type != VirtualPathType.Absolute || other.Type != VirtualPathType.Absolute) return null;
            return GetSubPathInternal(other, ignoreCase, VirtualPathType.AppRelative);
        }

        private VirtualPath GetSubPathInternal(VirtualPath other, bool ignoreCase, VirtualPathType type)
        {
            if (this.Parts.Count > other.Parts.Count) return null;
            var comparison = ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
            for (var i = 0; i < this.Parts.Count; i++) {
                if (!this.Parts[i].Equals(other.Parts[i], comparison)) return null;
            }
            return new VirtualPath(other.Parts.Skip(this.Parts.Count), type);
        }


        /* ====== Object method overrides ====== */

        public override string ToString()
        {
            switch (Type) {
                case VirtualPathType.Absolute:
                    return "/" + Path;
                case VirtualPathType.AppRelative:
                    return "~/" + Path;
                default:
                    return Path;
            }
        }

        public override bool Equals(object obj)
        {
            var other = obj as VirtualPath;
            if (other == null) return false;
            return this.Path.Equals(other.Path) && this.Type.Equals(other.Type);
        }

        public override int GetHashCode()
        {
            return this.Path.GetHashCode() ^ this.Type.GetHashCode();
        }
    }
}
