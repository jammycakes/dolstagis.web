using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web
{
    public class ErrorStatus : Status
    {
        /// <summary>
        ///  Throws an exception with this status code.
        /// </summary>

        public void Throw()
        {
            throw new HttpStatusException(this);
        }

        internal ErrorStatus(int code, string description)
            : base(code, description)
        { }

        internal ErrorStatus(int code, string description, string message)
            : base(code, description, message)
        { }
    }
}
