using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Http;

namespace Dolstagis.Web.Lifecycle
{
    public class RequestProcessor : IRequestProcessor
    {
        private IList<IResultProcessor> _resultProcessors;
        private IExceptionHandler _exceptionHandler;
        private IRequestContextBuilder _contextBuilder;
        private ISessionCookieBuilder _sessionCookieBuilder;

        public RequestProcessor(
            IEnumerable<IResultProcessor> resultProcessors,
            IExceptionHandler exceptionHandler,
            IRequestContextBuilder contextBuilder,
            ISessionCookieBuilder sessionCookieBuilder
        )
        {
            _resultProcessors = (resultProcessors ?? Enumerable.Empty<IResultProcessor>()).ToList();
            _exceptionHandler = exceptionHandler;
            _contextBuilder = contextBuilder;
            _sessionCookieBuilder = sessionCookieBuilder;
        }


        public async Task<object> InvokeRequest(IRequestContext context)
        {
            if (context == null) Status.NotFound.Throw();
            var action = context.Action;
            if (action == null) Status.NotFound.Throw();
            if (action.Method == null) Status.MethodNotAllowed.Throw();
            var result = action.Invoke(context);
            if (result == null) Status.NotFound.Throw();
            if (result is Task) {
                await (Task)result;
                return ((dynamic)result).Result;
            }
            else {
                return result;
            }
        }

        public async Task ProcessRequest(IRequestContext context)
        {
            object result;
            IResultProcessor processor;

            try
            {
                result = await InvokeRequest(context);
                processor = _resultProcessors.LastOrDefault(x => x.CanProcess(result));
                if (processor == null) Status.NotFound.Throw();
            }
            finally
            {
                var cookie = _sessionCookieBuilder.CreateSessionCookie(context.Request.SessionID);
                if (cookie != null)
                {
                    cookie.Secure = context.Request.IsSecure;
                    context.Response.AddCookie(cookie);
                }
            }
            await processor.Process(result, context);
        }

        public async Task ProcessRequest(Request request, Response response)
        {
            var context = _contextBuilder.CreateContext(request, response);
            Exception fault = null;
            try {
                await ProcessRequest(context);
            }
            catch (Exception ex) {
                fault = ex;
            }

            if (fault != null) {
                while (fault is AggregateException && ((AggregateException)fault).InnerExceptions.Count == 1) {
                    fault = ((AggregateException)fault).InnerExceptions.Single();
                }
                await _exceptionHandler.HandleException(context, fault);
            }
        }
    }
}
