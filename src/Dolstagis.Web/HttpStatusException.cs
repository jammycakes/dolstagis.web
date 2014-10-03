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
        public ErrorStatus Status { get; private set; }

        public HttpStatusException() : base() { }

        public HttpStatusException(ErrorStatus status)
            : base(status.ToString())
        {
            this.Status = status;
        }

        public HttpStatusException(ErrorStatus status, Exception innerException)
            : base(status.ToString(), innerException)
        {
            this.Status = status;
        }

        public HttpStatusException(Exception innerException)
            : this(Dolstagis.Web.Status.InternalServerError, innerException)
        { }

        protected HttpStatusException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
    }
}
