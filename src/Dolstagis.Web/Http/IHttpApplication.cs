using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dolstagis.Web.Http
{
    /// <summary>
    ///  Basic information about the HTTP application.
    /// </summary>

    public interface IHttpApplication
    {
        /// <summary>
        ///  Gets the virtual path to the application root.
        /// </summary>

        string Root { get; }

        /// <summary>
        ///  Gets the physical path to the application root.
        /// </summary>

        string PhysicalRoot { get; }
    }
}
