using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Http;

namespace Dolstagis.Web.ContentNegotiation
{
    public class Negotiation
    {
        public Match Match { get; private set; }

        public double Quality { get; private set; }

        public INegotiator Negotiator { get; private set; }

        public Func<IResult> GetResult { get; private set; }

        public Negotiation(Match match, double quality,
            INegotiator negotiator, Func<IResult> getResult)
        {
            Match = match;
            Quality = quality;
            Negotiator = negotiator;
            GetResult = getResult;
        }

        public static Negotiation None(INegotiator negotiator)
        {
            return new Negotiation(Match.None, 0, negotiator, () => null);
        }
    }
}
