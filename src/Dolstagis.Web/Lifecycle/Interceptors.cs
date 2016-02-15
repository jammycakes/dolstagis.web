using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.Lifecycle
{
    public class Interceptors
    {
        private IList<IInterceptor> _interceptors;

        public Interceptors()
        {
            _interceptors = new List<IInterceptor>();
        }

        public Interceptors(IEnumerable<IInterceptor> interceptors)
        {
            _interceptors = interceptors?.ToList() ?? new List<IInterceptor>();
        }

        public async Task<IRequestContext> BeginRequest(IRequestContext context)
        {
            foreach (var interceptor in _interceptors) {
                context = await interceptor.BeginRequest(context);
            }
            return context;
        }

        public async Task<object> ControllerFound(IRequestContext context, object controller)
        {
            foreach (var interceptor in _interceptors) {
                controller = await interceptor.ControllerFound(context, controller);
            }
            return controller;
        }

        public async Task<object[]> ModelBound(IRequestContext context, object[] model, MethodInfo method)
        {
            foreach (var interceptor in _interceptors) {
                model = await interceptor.ModelBound(context, model, method);
            }
            return model;
        }

        public async Task<object> ControllerResult(IRequestContext context, object controller, object result)
        {
            foreach (var interceptor in _interceptors) {
                result = await interceptor.ControllerResult(context, controller, result);
            }
            return result;
        }


        public async Task<Exception> Exception(IRequestContext context, Exception ex)
        {
            foreach (var interceptor in _interceptors) {
                ex = await interceptor.Exception(context, ex);
            }
            return ex;
        }

        public async Task EndRequest(IRequestContext context)
        {
            foreach (var interceptor in _interceptors) {
                await interceptor.EndRequest(context);
            }
        }
    }
}
