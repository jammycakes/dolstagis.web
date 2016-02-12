using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.Lifecycle.ResultProcessors
{
    public class TextResultProcessor : IResultProcessor
    {
        public MatchResult Match(object data, IRequestContext context)
        {
            var accept = context.Request.Headers.Accept;
            if (!accept.Any()) return MatchResult.Fallback;

            var result =
                from opt in context.Request.Headers.Accept
                let val = opt.Value.ToLowerInvariant()
                let q = opt.Q
                let isExact = val == "text/plain"
                let isInexact = val == "*/*"
                where isExact || isInexact
                let mr = new MatchResult(isExact ? Lifecycle.Match.Exact : Lifecycle.Match.Inexact, q)
                orderby mr.Match descending, mr.Q descending
                select mr;

            return result.FirstOrDefault() ?? MatchResult.None;
        }

        public async Task ProcessBodyAsync(object data, IRequestContext context)
        {
            using (var writer = new StreamWriter(context.Response.Body, Encoding.UTF8))
                await writer.WriteAsync(data.ToString());
        }

        public async Task ProcessHeadersAsync(object data, IRequestContext context)
        {
            context.Response.Headers["Content-Type"] = new[] { "text/plain; charset=utf-8" };
            await Task.Yield();
        }
    }
}
