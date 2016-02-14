using System;

namespace Dolstagis.Web
{
    public class ErrorStatus : Status
    {
        /// <summary>
        ///  Throws an exception with this status code.
        /// </summary>

        public Exception CreateException()
        {
            return new HttpStatusException(this);
        }

        internal ErrorStatus(int code, string description)
            : base(code, description)
        { }

        internal ErrorStatus(int code, string description, string message)
            : base(code, description, message)
        { }
    }
}
