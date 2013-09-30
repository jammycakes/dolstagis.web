using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web
{
    [Serializable]
    public class HttpStatusException : Exception
    {
        public Status Status { get; private set; }

        public HttpStatusException() : base() { }

        public HttpStatusException(Status status)
            : base(status.ToString())
        {
            this.Status = status;
        }

        protected HttpStatusException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
    }
}
