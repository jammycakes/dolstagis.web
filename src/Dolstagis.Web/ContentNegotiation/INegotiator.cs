using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Http;

namespace Dolstagis.Web.ContentNegotiation
{
    public interface INegotiator
    {
        Negotiation Negotiate(IRequest request, object model);

        /// <summary>
        ///  Indicates the priority that this negotiator will have when tied
        ///  with other negotiators.
        /// </summary>
        int TieBreaker { get; }
    }
}
