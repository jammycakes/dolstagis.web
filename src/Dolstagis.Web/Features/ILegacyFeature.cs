using System;
using Dolstagis.Web.IoC;
using Dolstagis.Web.Static;

namespace Dolstagis.Web.Features
{
    public interface ILegacyFeature
    {
        void AddViews(VirtualPath path, IResourceLocation location);
        void AddViews(VirtualPath path, string physicalPath);
        void AddViews(VirtualPath path, Func<IIoCContainer, IResourceLocation> locationFactory);
    }
}