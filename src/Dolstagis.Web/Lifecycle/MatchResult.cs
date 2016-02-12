using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.Lifecycle
{
    public class MatchResult
    {
        public float Q { get; private set; }

        public Match Match { get; private set; }

        public MatchResult(Match match, float q)
        {
            Match = match;
            Q = q;
        }

        public static readonly MatchResult Exact = new MatchResult(Match.Exact, 1);
        public static readonly MatchResult Fallback = new MatchResult(Match.Fallback, 0);
        public static readonly MatchResult None = new MatchResult(Match.None, 1);
    }
}
