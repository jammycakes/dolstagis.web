using System;

namespace Dolstagis.Web.Logging
{
    public class LogEntry
    {
        public string Message { get; private set; }

        public Exception Exception { get; private set; }

        public LogEntry(string message, Exception exception)
        {
            this.Message = message;
            this.Exception = exception;
        }
    }
}
