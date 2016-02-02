using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Dolstagis.Web.Logging
{
    public abstract class Logger
    {
        public static LogEntry Entry(string message, Exception exception = null)
        {
            return new LogEntry(message, exception);
        }

        public static ILoggingProvider Provider { get; set; }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static Logger ForThisClass()
        {
            var stackTrace = new StackTrace(1);
            var frame = stackTrace.GetFrame(0);
            var type = frame.GetMethod().DeclaringType;
            return Provider.CreateLogger(type);
        }

        static Logger()
        {
            if (NLogLoggingProvider.IsAvailable)
                Provider = new NLogLoggingProvider();
            else if (Log4netLoggingProvider.IsAvailable)
                Provider = new Log4netLoggingProvider();
            else
                Provider = new NullLoggingProvider();
        }


        public abstract bool IsFatalEnabled { get; }

        public abstract void Fatal(Func<LogEntry> messageFunc);

        public abstract bool IsErrorEnabled { get; }

        public abstract void Error(Func<LogEntry> messageFunc);

        public abstract bool IsWarnEnabled { get; }

        public abstract void Warn(Func<LogEntry> messageFunc);

        public abstract bool IsInfoEnabled { get; }
        public abstract void Info(Func<LogEntry> messageFunc);

        public abstract bool IsDebugEnabled { get; }

        public abstract void Debug(Func<LogEntry> messageFunc);

        public abstract bool IsTraceEnabled { get; }

        public abstract void Trace(Func<LogEntry> messageFunc);

        public void Fatal(string message)
        {
            Fatal(() => Entry(message));
        }

        public void Fatal(string message, Exception exception)
        {
            Fatal(() => Entry(message, exception));
        }

        public void Fatal(Func<string> messageFunc)
        {
            Fatal(() => Entry(messageFunc()));
        }

        public void Error(string message)
        {
            Error(() => Entry(message));
        }

        public void Error(string message, Exception exception)
        {
            Error(() => Entry(message, exception));
        }

        public void Error(Func<string> messageFunc)
        {
            Error(() => Entry(messageFunc()));
        }

        public void Warn(string message)
        {
            Warn(() => Entry(message));
        }

        public void Warn(string message, Exception exception)
        {
            Warn(() => Entry(message, exception));
        }

        public void Warn(Func<string> messageFunc)
        {
            Warn(() => Entry(messageFunc()));
        }


        public void Info(string message)
        {
            Info(() => Entry(message));
        }

        public void Info(string message, Exception exception)
        {
            Info(() => Entry(message, exception));
        }

        public void Info(Func<string> messageFunc)
        {
            Info(() => Entry(messageFunc()));
        }

        public void Debug(string message)
        {
            Debug(() => Entry(message));
        }

        public void Debug(string message, Exception exception)
        {
            Debug(() => Entry(message, exception));
        }

        public void Debug(Func<string> messageFunc)
        {
            Debug(() => Entry(messageFunc()));
        }
        
        public void Trace(string message)
        {
            Trace(() => Entry(message));
        }

        public void Trace(string message, Exception exception)
        {
            Trace(() => Entry(message, exception));
        }

        public void Trace(Func<string> messageFunc)
        {
            Trace(() => Entry(messageFunc()));
        }
    }
}
