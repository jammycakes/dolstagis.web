using System;

namespace Dolstagis.Web.Logging
{
    public class NullLoggingProvider : ILoggingProvider
    {
        public Logger CreateLogger(Type type)
        {
            return new NullLogger(type.Name);
        }
    }
}
