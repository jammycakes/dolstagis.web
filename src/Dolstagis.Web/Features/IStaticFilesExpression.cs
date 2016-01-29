using System;
using System.IO;
using System.Reflection;

namespace Dolstagis.Web.Features
{
    public interface IStaticFilesExpression
    {
        void FromStream(Func<VirtualPath, IServiceProvider, Stream> streamLocator);
    }
}