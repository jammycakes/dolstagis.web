using System;
using System.Linq;

namespace Dolstagis.Web.Static
{
    public class ResourceMapping
    {
        public VirtualPath Root { get; private set; }

        public IResourceLocation Location { get; private set; }

        public ResourceMapping(VirtualPath root, IResourceLocation location)
        {
            if (root.Type != VirtualPathType.AppRelative) {
                throw new ArgumentException("The root path must be app-relative.");
            }
            Root = root;
            Location = location;
        }

        public IResource GetResource(VirtualPath path)
        {
            switch (path.Type) {
                case VirtualPathType.Absolute:
                    throw new ArgumentException("The path to locate must be app-relative.");
                case VirtualPathType.AppRelative:
                    path = Root.GetSubPath(path, true);
                    if (path == null) {
                        throw new ArgumentException("The path to locate must be a sub-path of the root.");
                    }
                    break;
            }
            if (path.Parts.FirstOrDefault() == "..") {
                throw new ArgumentException("The path to locate must be a sub-path of the root.");
            }

            return Location.GetResource(path);
        }
    }
}
