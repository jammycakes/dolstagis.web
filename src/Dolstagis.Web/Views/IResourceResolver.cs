using System;
using Dolstagis.Web.Static;

namespace Dolstagis.Web.Views
{
    public interface IResourceResolver
    {
        IResource GetResource(VirtualPath path);
    }
}