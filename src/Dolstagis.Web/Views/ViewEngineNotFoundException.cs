using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.Views
{
    [Serializable]
    public class ViewEngineNotFoundException : Exception
    {
        public ViewEngineNotFoundException()
            : base()
        { }

        public ViewEngineNotFoundException(string message)
            : base(message)
        { }

        public ViewEngineNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        { }

        protected ViewEngineNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
    }
}
