using System;

namespace Dolstagis.Web.Logging
{
    public class NullLogger : Logger
    {
        public string Name { get; private set; }

        public NullLogger(string name)
        {
            this.Name = name;
        }

        public override void Fatal(Func<LogEntry> messageFunc)
        {
        }

        public override void Error(Func<LogEntry> messageFunc)
        {
        }

        public override void Warn(Func<LogEntry> messageFunc)
        {
        }

        public override void Info(Func<LogEntry> messageFunc)
        {
        }

        public override void Debug(Func<LogEntry> messageFunc)
        {
        }

        public override void Trace(Func<LogEntry> messageFunc)
        {
        }

        public override bool IsFatalEnabled
        {
            get { return false; }
        }

        public override bool IsErrorEnabled
        {
            get { return false; }
        }

        public override bool IsWarnEnabled
        {
            get { return false; }
        }

        public override bool IsInfoEnabled
        {
            get { return false; }
        }

        public override bool IsDebugEnabled
        {
            get { return false; }
        }

        public override bool IsTraceEnabled
        {
            get { return false; }
        }
    }
}
