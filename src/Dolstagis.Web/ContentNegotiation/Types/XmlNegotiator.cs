using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Dolstagis.Web.Http;

namespace Dolstagis.Web.ContentNegotiation.Types
{
    public class XmlNegotiator : INegotiator
    {
        public int TieBreaker
        {
            get { return 0; }
        }

        private static readonly Regex reIsXml = new Regex(@"^(application|text)/xml$");

        public Negotiation Negotiate(IRequest request, object model)
        {
            Match match;

            var opt = request.Headers.MatchAccept(reIsXml);
            if (opt == null)
                return Negotiation.None(this);
            else if (opt.Value == "*/*")
                match = Match.Inexact;
            else
                match = Match.Exact;
            return new Negotiation(
                match, opt.Q, this,
                () => {
                    var result = new XmlResult(model);
                    if (opt.Value != "*/*") result.MimeType = opt.Value;
                    return result;
                }
            );
        }
    }
}
