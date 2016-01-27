using System;

namespace Dolstagis.Web.Logging
{
    public interface ILoggingProvider
    {
        Logger CreateLogger(Type type);
    }
}
