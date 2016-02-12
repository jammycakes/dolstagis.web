using System;
using System.Linq.Expressions;

namespace Dolstagis.Web.Logging
{
    internal class NLogLoggerFactory : ILoggerFactory
    {
        private static readonly Type logManagerType = Type.GetType("NLog.LogManager, NLog", false);
        private static readonly Type loggerType = Type.GetType("NLog.Logger, NLog", false);

        private static readonly Func<string, object> getLoggerDelegate;

        public static bool IsAvailable
        {
            get { return logManagerType != null && loggerType != null; }
        }


        static NLogLoggerFactory()
        {
            if (!IsAvailable) return;

            var method = logManagerType.GetMethod("GetLogger", new[] { typeof(string) });
            ParameterExpression resultValue;
            ParameterExpression keyParam = Expression.Parameter(typeof(string));
            MethodCallExpression methodCall = Expression.Call
                (null, method, new Expression[] { resultValue = keyParam });
            getLoggerDelegate = Expression
                .Lambda<Func<string, object>>(methodCall, new[] { resultValue })
                .Compile();
        }


        public Logger CreateLogger(Type type)
        {
            return new NLogLogger(getLoggerDelegate(type.FullName));
        }
    }
}
