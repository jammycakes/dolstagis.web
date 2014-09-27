using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.Logging
{
    internal class NLogLogger : Logger
    {
        private static readonly Type loggerType = Type.GetType("NLog.Logger, NLog", false);

        private static readonly Func<object, bool> IsFatalEnabledDelegate;
        private static readonly Func<object, bool> IsErrorEnabledDelegate;
        private static readonly Func<object, bool> IsWarnEnabledDelegate;
        private static readonly Func<object, bool> IsInfoEnabledDelegate;
        private static readonly Func<object, bool> IsDebugEnabledDelegate;
        private static readonly Func<object, bool> IsTraceEnabledDelegate;

        private static readonly Action<object, LogEntry> LogFatalDelegate;
        private static readonly Action<object, LogEntry> LogErrorDelegate;
        private static readonly Action<object, LogEntry> LogWarnDelegate;
        private static readonly Action<object, LogEntry> LogInfoDelegate;
        private static readonly Action<object, LogEntry> LogDebugDelegate;
        private static readonly Action<object, LogEntry> LogTraceDelegate;

        static NLogLogger()
        {
            IsFatalEnabledDelegate = GetPropertyGetter("IsFatalEnabled");
            IsErrorEnabledDelegate = GetPropertyGetter("IsErrorEnabled");
            IsWarnEnabledDelegate = GetPropertyGetter("IsWarnEnabled");
            IsInfoEnabledDelegate = GetPropertyGetter("IsInfoEnabled");
            IsDebugEnabledDelegate = GetPropertyGetter("IsDebugEnabled");
            IsTraceEnabledDelegate = GetPropertyGetter("IsTraceEnabled");

            LogFatalDelegate = GetLogEntryDelegate("Fatal");
            LogErrorDelegate = GetLogEntryDelegate("Error");
            LogWarnDelegate = GetLogEntryDelegate("Warn");
            LogInfoDelegate = GetLogEntryDelegate("Info");
            LogDebugDelegate = GetLogEntryDelegate("Debug");
            LogTraceDelegate = GetLogEntryDelegate("Trace");
        }


        private static Func<object, bool> GetPropertyGetter(string propertyName)
        {
            ParameterExpression param = Expression.Parameter(typeof(object));
            Expression convertedParam = Expression.Convert(param, loggerType);
            Expression property = Expression.Property(convertedParam, propertyName);
            return (Func<object, bool>)Expression.Lambda(property, param).Compile();
        }


        private static Action<object, string> GetLogDelegate(string methodName)
        {
            var loggerParam = Expression.Parameter(typeof(object));
            var messageParam = Expression.Parameter(typeof(string));
            Expression convertedParam = Expression.Convert(loggerParam, loggerType);
            var method = loggerType.GetMethod(methodName, new[] { typeof(object) });
            MethodCallExpression methodCall = Expression.Call(convertedParam, method, messageParam);
            return (Action<object, string>)Expression
                .Lambda(methodCall, new[] { loggerParam, messageParam })
                .Compile();
        }


        private static Action<object, string, Exception> GetLogExceptionDelegate(string methodName)
        {
            var loggerParam = Expression.Parameter(typeof(object));
            var messageParam = Expression.Parameter(typeof(string));
            var exceptionParam = Expression.Parameter(typeof(Exception));
            Expression convertedParam = Expression.Convert(loggerParam, loggerType);
            var method = loggerType.GetMethod(methodName, new[] { typeof(string), typeof(Exception) });
            MethodCallExpression methodCall = Expression.Call
                (convertedParam, method, messageParam, exceptionParam);
            return (Action<object, string, Exception>)Expression
                .Lambda(methodCall, new[] { loggerParam, messageParam, exceptionParam })
                .Compile();
        }


        private static Action<object, LogEntry> GetLogEntryDelegate(string methodName)
        {
            var logDelegate = GetLogDelegate(methodName);
            var logExceptionDelegate = GetLogExceptionDelegate(methodName + "Exception");

            return (object logger, LogEntry entry) =>
            {
                if (entry.Exception != null)
                    logExceptionDelegate(logger, entry.Message, entry.Exception);
                else
                    logDelegate(logger, entry.Message);
            };
        }



        private object loggerObject;

        public NLogLogger(object loggerObject)
        {
            this.loggerObject = loggerObject;
        }


        public override bool IsFatalEnabled
        {
            get { return IsFatalEnabledDelegate(loggerObject); }
        }

        public override void Fatal(Func<LogEntry> messageFunc)
        {
            if (IsFatalEnabled) LogFatalDelegate(loggerObject, messageFunc());
        }

        public override bool IsErrorEnabled
        {
            get { return IsErrorEnabledDelegate(loggerObject); }
        }

        public override void Error(Func<LogEntry> messageFunc)
        {
            if (IsErrorEnabled) LogErrorDelegate(loggerObject, messageFunc());
        }

        public override bool IsWarnEnabled
        {
            get { return IsWarnEnabledDelegate(loggerObject); }
        }

        public override void Warn(Func<LogEntry> messageFunc)
        {
            if (IsWarnEnabled) LogWarnDelegate(loggerObject, messageFunc());
        }

        public override bool IsInfoEnabled
        {
            get { return IsInfoEnabledDelegate(loggerObject); }
        }

        public override void Info(Func<LogEntry> messageFunc)
        {
            if (IsInfoEnabled) LogInfoDelegate(loggerObject, messageFunc());
        }

        public override bool IsDebugEnabled
        {
            get { return IsDebugEnabledDelegate(loggerObject); }
        }

        public override void Debug(Func<LogEntry> messageFunc)
        {
            if (IsDebugEnabled) LogDebugDelegate(loggerObject, messageFunc());
        }

        public override bool IsTraceEnabled
        {
            get { return IsTraceEnabledDelegate(loggerObject); }
        }

        public override void Trace(Func<LogEntry> messageFunc)
        {
            if (IsTraceEnabled) LogTraceDelegate(loggerObject, messageFunc());
        }
    }
}
