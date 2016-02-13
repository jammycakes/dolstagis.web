using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Http;

namespace Dolstagis.Web.ContentNegotiation.Types
{
    public class TextContentNegotiator : INegotiator
    {
        public int TieBreaker
        {
            get { return int.MaxValue; }
        }

        public Negotiation Negotiate(IRequest request, object model)
        {
            Match match;

            var opt = request.Headers.MatchAccept("text/plain");
            if (opt == null)
                match = Match.Fallback;
            else if (opt.Value == "*/*")
                match = Match.Inexact;
            else
                match = Match.Exact;
            return new Negotiation(
                match, opt.Q, this,
                () => new ContentResult(model.ToString())
            );
        }
    }
}
