namespace Dolstagis.Web.Lifecycle
{
    public class MatchResult
    {
        public double Q { get; private set; }

        public Match Match { get; private set; }

        public string Value { get; private set; }

        public MatchResult(Match match, double q, string value)
        {
            Match = match;
            Q = q;
            Value = value;
        }

        public static readonly MatchResult Exact = new MatchResult(Match.Exact, 1, null);
        public static readonly MatchResult Fallback = new MatchResult(Match.Fallback, 0, null);
        public static readonly MatchResult None = new MatchResult(Match.None, 0, null);
    }
}
