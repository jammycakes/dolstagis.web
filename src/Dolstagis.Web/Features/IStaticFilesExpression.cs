using System;
using Dolstagis.Web.IoC;
using Dolstagis.Web.Static;

namespace Dolstagis.Web.Features
{
    public interface IStaticFilesExpression
    {
        void FromResource(Func<VirtualPath, IServiceLocator, IResource> locator);
    }
}