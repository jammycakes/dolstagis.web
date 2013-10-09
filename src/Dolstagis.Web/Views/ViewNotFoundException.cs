using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.Views
{
    [Serializable]
    public class ViewNotFoundException : Exception
    {
        public ViewNotFoundException()
            : base()
        { }

        public ViewNotFoundException(string message)
            : base(message)
        { }

        public ViewNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        { }

        protected ViewNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
    }
}
