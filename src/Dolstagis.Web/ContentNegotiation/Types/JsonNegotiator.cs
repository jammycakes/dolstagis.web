using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Dolstagis.Web.Http;

namespace Dolstagis.Web.ContentNegotiation.Types
{
    public class JsonNegotiator : INegotiator
    {
        private static readonly Regex reIsJson = new Regex(@"^application/(.*\+)?json$");

        public int TieBreaker
        {
            get { return 0; }
        }

        public Negotiation Negotiate(IRequest request, object model)
        {
            var opt = request.Headers.MatchAccept(reIsJson);
            if (opt == null) return Negotiation.None(this);
            var match = opt.Value == "*/*" ? Match.Inexact : Match.Exact;
            return new Negotiation(
                match, opt.Q, this, () => {
                    var result = new JsonResult(model);
                    if (opt.Value != "*/*") result.MimeType = opt.Value;
                    return result;
                }
            );
        }
    }
}
