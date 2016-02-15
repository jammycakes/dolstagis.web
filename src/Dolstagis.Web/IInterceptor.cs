using System;
using System.Threading.Tasks;

namespace Dolstagis.Web
{
    public interface IInterceptor
    {
        /// <summary>
        ///  Invoked at the start of each request, after the request context has
        ///  been initialised but before the controller has been resolved. This
        ///  method allows us to make changes to the request context prior to
        ///  processing the request.
        /// </summary>
        /// <param name="context">
        ///  The request context.
        /// </param>
        /// <returns>
        ///  The reques context, or an alternative <see cref="IRequestContext"/>
        ///  implementation if appropriate.
        /// </returns>
        Task<IRequestContext> BeginRequest(IRequestContext context);

        /// <summary>
        ///  Invoked once the controller has been resolved.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="controller"></param>
        /// <returns></returns>
        Task<object> ControllerFound(IRequestContext context, object controller);

        Task<Exception> HandleException(IRequestContext context, Exception ex);
    }
}
