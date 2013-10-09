using System;

namespace Dolstagis.Web.Static
{
    public interface IResourceLocator
    {
        IResource GetResource(Dolstagis.Web.VirtualPath path);
    }
}
