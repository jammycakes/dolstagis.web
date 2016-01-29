using System;
using Dolstagis.Web.Routes;
using Dolstagis.Web.Static;
using StructureMap;

namespace Dolstagis.Web.Features
{
    public interface ILegacyFeature
    {
        IModelBinder ModelBinder { get; set; }
        Registry Services { get; }

        void AddController<T>() where T : Controller;
        void AddController<T>(VirtualPath route) where T : Controller;
        void AddStaticFiles(VirtualPath path, IResourceLocation location);
        void AddStaticFiles(VirtualPath path, string physicalPath);
        void AddStaticFiles(VirtualPath path, Func<IContext, IResourceLocation> locationFactory);
        void AddViews(VirtualPath path, IResourceLocation location);
        void AddViews(VirtualPath path, string physicalPath);
        void AddViews(VirtualPath path, Func<IContext, IResourceLocation> locationFactory);
    }
}