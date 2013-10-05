using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.Static
{
    public abstract class ResourceLocation
    {
        public VirtualPath Root { get; private set; }

        protected abstract IResource CreateResource(VirtualPath path);

        protected ResourceLocation(VirtualPath root)
        {
            if (root.Type != VirtualPathType.AppRelative) {
                throw new ArgumentException("The root path must be app-relative.");
            }
            Root = root;
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

            return CreateResource(path);
        }
    }
}
