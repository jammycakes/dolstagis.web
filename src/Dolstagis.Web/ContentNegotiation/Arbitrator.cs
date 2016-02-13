using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Http;

namespace Dolstagis.Web.ContentNegotiation
{
    public class Arbitrator : IArbitrator
    {
        private IList<INegotiator> _negotiators;

        public Arbitrator(IEnumerable<INegotiator> negotiators)
        {
            _negotiators = negotiators.ToList();
        }

        public Negotiation GetBestMatch(IRequest request, object model)
        {
            var negotiations = (
                from negotiator in _negotiators
                let negotiation = negotiator.Negotiate(request, model)
                where negotiation.Match != Match.None
                orderby negotiation.Match descending,
                    negotiation.Quality descending,
                    negotiation.Negotiator.TieBreaker descending
                select negotiation
            ).ToList();
            return negotiations.FirstOrDefault();
        }

        public IResult Arbitrate(IRequest request, object model)
        {
            return GetBestMatch(request, model)?.GetResult();
        }
    }
}
