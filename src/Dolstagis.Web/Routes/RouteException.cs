using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.Routes
{
    [Serializable]
    public class RouteException : Exception
    {
        public RouteException() : base() { }

        public RouteException(string message) : base(message) { }

        public RouteException(string message, Exception innerException)
            : base(message, innerException)
        { }

        protected RouteException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
    }
}
