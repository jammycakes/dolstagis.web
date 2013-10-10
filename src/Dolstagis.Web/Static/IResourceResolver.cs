using System;

namespace Dolstagis.Web.Static
{
    public interface IResourceResolver
    {
        IResource GetResource(Dolstagis.Web.VirtualPath path);
    }
}
