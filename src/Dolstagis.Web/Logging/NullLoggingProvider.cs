using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
