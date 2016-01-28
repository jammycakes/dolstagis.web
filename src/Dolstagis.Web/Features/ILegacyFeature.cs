using System;
using Dolstagis.Web.Routes;
using Dolstagis.Web.Static;
using StructureMap;

namespace Dolstagis.Web.Features
{
    public interface ILegacyFeature
    {
        IModelBinder ModelBinder { get; set; }
        IRouteTable Routes { get; }
        Registry Services { get; }

        void AddHandler<T>() where T : Handler;
        void AddHandler<T>(string route) where T : Handler;
        void AddStaticFiles(VirtualPath vPath, IResourceLocation location);
        void AddStaticFiles(VirtualPath path, string physicalPath);
        void AddStaticFiles(VirtualPath vPath, Func<IContext, IResourceLocation> locationFactory);
        void AddStaticFiles(string path, string physicalPath);
        void AddStaticFiles(string path, IResourceLocation location);
        void AddStaticFiles(string path, Func<IContext, IResourceLocation> locationFactory);
        void AddViews(VirtualPath vPath, IResourceLocation location);
        void AddViews(VirtualPath path, string physicalPath);
        void AddViews(VirtualPath vPath, Func<IContext, IResourceLocation> locationFactory);
        void AddViews(string path, string physicalPath);
        void AddViews(string path, IResourceLocation location);
        void AddViews(string path, Func<IContext, IResourceLocation> locationFactory);
    }
}