using System;
using Dolstagis.Web.Static;

namespace Dolstagis.Web.Features
{
    public interface ILegacyFeature
    {
        IModelBinder ModelBinder { get; set; }

        void AddController<T>();
        void AddController<T>(VirtualPath route);
        void AddStaticFiles(VirtualPath path, IResourceLocation location);
        void AddStaticFiles(VirtualPath path, string physicalPath);
        void AddStaticFiles(VirtualPath path, Func<IIoCContainer, IResourceLocation> locationFactory);
        void AddViews(VirtualPath path, IResourceLocation location);
        void AddViews(VirtualPath path, string physicalPath);
        void AddViews(VirtualPath path, Func<IIoCContainer, IResourceLocation> locationFactory);
    }
}