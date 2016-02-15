using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web
{
    /// <summary>
    ///  Provides a base class for the <see cref="IInterceptor"/> interface
    ///  which allows you to selectively override some extension points
    ///  while leaving others unchanged.
    /// </summary>

    public class Interceptor : IInterceptor
    {
        public virtual Task<IRequestContext> BeginRequest(IRequestContext context)
        {
            return Task.FromResult(context);
        }

        public Task<object> ControllerFound(IRequestContext context, object controller)
        {
            return Task.FromResult(controller);
        }

        public Task<object[]> ModelBound(IRequestContext context, object[] model, MethodInfo method)
        {
            return Task.FromResult(model);
        }

        public Task<Exception> HandleException(IRequestContext context, Exception ex)
        {
            return Task.FromResult(ex);
        }
    }
}
