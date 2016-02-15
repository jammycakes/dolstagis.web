using System;
using System.Reflection;
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

        /// <summary>
        ///  Called when the parameters have been bound to the model passed into
        ///  the controller action.
        /// </summary>
        /// <param name="context">
        ///  The request context.
        /// </param>
        /// <param name="model">
        ///  The controller action parameters bound from the request data.
        /// </param>
        /// <param name="method">
        ///  The controller action method to call.
        /// </param>
        /// <returns>
        ///  The controller action parameters, with any required modifications.
        /// </returns>
        Task<object[]> ModelBound(IRequestContext context, object[] model, MethodInfo method);

        /// <summary>
        ///  Called when the controller has successfully returned a result.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="controller"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        Task<object> ControllerResult(IRequestContext context, object controller, object result);

        Task<Exception> Exception(IRequestContext context, Exception ex);
    }
}
