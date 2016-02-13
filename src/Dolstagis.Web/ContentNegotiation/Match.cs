using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.ContentNegotiation
{
    public enum Match
    {
        /// <summary>
        ///  The content negotiator can not handle this request.
        /// </summary>
        None,

        /// <summary>
        ///  The content negotiator can handle any request. End of story.
        /// </summary>
        Fallback,

        /// <summary>
        ///  The content negotiator can handle this request, but is not an exact
        ///  match.
        /// </summary>
        Inexact,

        /// <summary>
        ///  The content negotiator can handle this request,
        ///  and is an exact match.
        /// </summary>
        Exact
    }
}
