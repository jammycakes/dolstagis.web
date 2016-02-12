using System.Threading.Tasks;
using Dolstagis.Web.Lifecycle;

namespace Dolstagis.Web
{
    public interface IResultProcessor
    {
        /// <summary>
        ///  Gets a value indicating whether this processor can handle a result of this type.
        /// </summary>
        /// <param name="data">The data or view returned from the controller.</param>
        /// <returns>
        ///  true if this processor can handle data of this type, otherwise false.
        /// </returns>

        Match Match(object data, IRequestContext context);

        /// <summary>
        ///  Processes the data returned from the controller and sends the output to the response.
        /// </summary>
        /// <param name="data">
        ///  The data to be processed.
        /// </param>
        /// <param name="context">
        ///  The <see cref="IRequestContext"/> instance.
        /// </param>
        /// <returns>A <see cref="Task"/> instance.</returns>

        Task ProcessHeadersAsync(object data, IRequestContext context);

        Task ProcessBodyAsync(object data, IRequestContext context);
    }
}
