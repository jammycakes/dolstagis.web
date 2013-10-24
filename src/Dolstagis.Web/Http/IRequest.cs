using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Dolstagis.Web.Http
{
    /// <summary>
    ///  Represents a raw HTTP request.
    /// </summary>

    public interface IRequest
    {
        /// <summary>
        ///  Gets the HTTP method.
        /// </summary>

        string Method { get; }

        /// <summary>
        ///  Gets the full path passed in the request.
        /// </summary>

        VirtualPath Path { get; }

        /// <summary>
        ///  Gets the path relative to the application root.
        ///  This MUST NOT start with a slash.
        /// </summary>

        VirtualPath AppRelativePath { get; }

        /// <summary>
        ///  Gets the protocol and version (HTTP/1.0 or HTTP/1.1)
        /// </summary>

        string Protocol { get; }

        /// <summary>
        ///  Gets a value indicating whether or not HTTPS is used.
        /// </summary>

        bool IsSecure { get; }

        /// <summary>
        ///  Gets the URL of the current request.
        /// </summary>

        Uri Url { get; }

        /// <summary>
        ///  Gets the query string components.
        /// </summary>

        NameValueCollection Query { get; }

        /// <summary>
        ///  Gets the form values.
        /// </summary>

        NameValueCollection Form { get; }

        /// <summary>
        ///  Gets the HTTP headers.
        /// </summary>
        NameValueCollection Headers { get; }
    }
}
