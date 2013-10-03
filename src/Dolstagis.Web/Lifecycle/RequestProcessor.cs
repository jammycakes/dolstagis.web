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

        public RequestProcessor(
            IEnumerable<IResultProcessor> resultProcessors,
            IExceptionHandler exceptionHandler,
            IRequestContextBuilder contextBuilder)
        {
            _resultProcessors = (resultProcessors ?? Enumerable.Empty<IResultProcessor>()).ToList();
            _exceptionHandler = exceptionHandler;
            _contextBuilder = contextBuilder;
        }


        public async Task<object> InvokeRequest(IRequestContext context)
        {
            if (context == null) Status.NotFound.Throw();
            var action = context.Action;
            if (action == null) Status.NotFound.Throw();
            if (action.Method == null) Status.MethodNotAllowed.Throw();
            var result = action.Invoke();
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
            var result = await InvokeRequest(context);
            var processor = _resultProcessors.LastOrDefault(x => x.CanProcess(result));
            if (processor == null) Status.NotFound.Throw();
            await processor.Process(result, context);
        }

        public async Task ProcessRequest(IRequest request, IResponse response)
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
