using System;
using System.IO;
using Dolstagis.Web.Static;

namespace Dolstagis.Web.Features
{
    public interface IStaticFilesExpression
    {
        void FromResource(Func<VirtualPath, IServiceProvider, IResource> locator);
    }
}