using System.IO;

namespace Dolstagis.Web.Http
{
    /// <summary>
    ///  Provides an interface to the raw HTTP response.
    /// </summary>

    public interface IResponse
    {
        /// <summary>
        ///  Adds a header to the response.
        /// </summary>
        /// <param name="name">
        ///  The name of the header to send.
        /// </param>
        /// <param name="value">
        ///  The value of the header to send.
        /// </param>

        void AddHeader(string name, string value);

        /// <summary>
        ///  Gets the response stream.
        /// </summary>

        Stream Body { get; }

        /// <summary>
        ///  Gets the response headers.
        /// </summary>

        ResponseHeaders Headers { get; }

        /// <summary>
        ///  Gets or sets the response status.
        /// </summary>

        Status Status { get; set; }

        /// <summary>
        ///  Gets or sets the HTTP protocol used for the response.
        /// </summary>

        string Protocol { get; set; }
    }
}
