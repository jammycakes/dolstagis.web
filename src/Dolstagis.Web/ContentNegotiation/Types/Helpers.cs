using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Dolstagis.Web.Http;

namespace Dolstagis.Web.ContentNegotiation.Types
{
    public static class Helpers
    {
        public static Option MatchAccept(this RequestHeaders headers, Predicate<string> test)
        {
            var opts =
                from opt in headers.Accept
                let exact = test(opt.Value)
                let wildcard = opt.Value == "*/*"
                where exact || wildcard
                orderby (exact ? 0 : 1), opt.Q descending
                select opt;
            return opts.FirstOrDefault();
        }

        public static Option MatchAccept(this RequestHeaders headers, Regex re)
        {
            return headers.MatchAccept(re.IsMatch);
        }

        public static Option MatchAccept(this RequestHeaders headers, string mime)
        {
            return headers.MatchAccept(s =>
                string.Compare(s, mime, StringComparison.OrdinalIgnoreCase) == 0
            );
        }
    }
}
