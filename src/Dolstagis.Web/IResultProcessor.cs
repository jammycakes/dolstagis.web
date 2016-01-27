using System.Threading.Tasks;

namespace Dolstagis.Web
{
    public interface IResultProcessor
    {
        /// <summary>
        ///  Gets a value indicating whether this processor can handle a result of this type.
        /// </summary>
        /// <param name="data">The data or view returned from the handler.</param>
        /// <returns>
        ///  true if this processor can handle data of this type, otherwise false.
        /// </returns>

        bool CanProcess(object data);

        /// <summary>
        ///  Processes the data returned from the handler and sends the output to the response.
        /// </summary>
        /// <param name="data">
        ///  The data to be processed.
        /// </param>
        /// <param name="context">
        ///  The <see cref="IRequestContext"/> instance.
        /// </param>
        /// <returns>A <see cref="Task"/> instance.</returns>

        Task Process(object data, RequestContext context);
    }
}
