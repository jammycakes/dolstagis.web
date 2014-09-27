using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.Logging
{
    internal class Log4netLoggingProvider : ILoggingProvider
    {
        private static readonly Type logManagerType = Type.GetType("log4net.LogManager, log4net", false);
        private static readonly Type loggerType = Type.GetType("log4net.ILog, log4net", false);

        private static readonly Func<Type, object> getLoggerDelegate;

        public static bool IsAvailable
        {
            get { return logManagerType != null && loggerType != null; }
        }


        static Log4netLoggingProvider()
        {
            if (!IsAvailable) return;

            var method = logManagerType.GetMethod("GetLogger", new[] { typeof(Type) });
            ParameterExpression resultValue;
            ParameterExpression keyParam = Expression.Parameter(typeof(Type));
            MethodCallExpression methodCall = Expression.Call
                (null, method, new Expression[] { resultValue = keyParam });
            getLoggerDelegate = Expression
                .Lambda<Func<Type, object>>(methodCall, new[] { resultValue })
                .Compile();
        }


        public Logger CreateLogger(Type type)
        {
            return new Log4netLogger(getLoggerDelegate(type));
        }
    }
}
