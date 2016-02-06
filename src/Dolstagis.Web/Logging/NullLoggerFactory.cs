using System;

namespace Dolstagis.Web.Logging
{
    public class NullLoggerFactory : ILoggerFactory
    {
        public Logger CreateLogger(Type type)
        {
            return new NullLogger(type.Name);
        }
    }
}
