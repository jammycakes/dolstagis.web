using System;

namespace Dolstagis.Web.Logging
{
    public interface ILoggerFactory
    {
        Logger CreateLogger(Type type);
    }
}
