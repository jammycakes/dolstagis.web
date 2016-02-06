using System;
using Dolstagis.Web.Static;

namespace Dolstagis.Web.Features
{
    public interface ILegacyFeature
    {
        IModelBinder ModelBinder { get; set; }

        void AddViews(VirtualPath path, IResourceLocation location);
        void AddViews(VirtualPath path, string physicalPath);
        void AddViews(VirtualPath path, Func<IIoCContainer, IResourceLocation> locationFactory);
    }
}