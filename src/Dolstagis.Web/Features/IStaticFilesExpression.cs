using System;
using System.IO;

namespace Dolstagis.Web.Features
{
    public interface IStaticFilesExpression
    {
        void FromStream(Func<VirtualPath, IServiceProvider, Stream> streamLocator);
    }
}