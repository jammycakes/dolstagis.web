using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.Lifecycle
{
    internal class Interceptors
    {
        private IList<IInterceptor> _interceptors;

        public Interceptors(IEnumerable<IInterceptor> interceptors)
        {
            _interceptors = interceptors.ToList();
        }

        public async Task<IRequestContext> BeginRequest(IRequestContext context)
        {
            foreach (var interceptor in _interceptors) {
                context = await interceptor.BeginRequest(context);
            }
            return context;
        }

        public async Task<Exception> HandleException(IRequestContext context, Exception ex)
        {
            foreach (var interceptor in _interceptors) {
                ex = await interceptor.HandleException(context, ex);
            }
            return ex;
        }
    }
}
