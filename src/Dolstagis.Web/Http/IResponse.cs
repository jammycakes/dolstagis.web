using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

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

        Stream ResponseStream { get; }

        /// <summary>
        ///  Gets or sets the response status.
        /// </summary>

        Status Status { get; set; }
    }
}
