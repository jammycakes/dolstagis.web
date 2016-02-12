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
        public Match Match(object data, IRequestContext context)
        {
            return Lifecycle.Match.Fallback;
        }

        public async Task ProcessBodyAsync(object data, IRequestContext context)
        {
            using (var writer = new StreamWriter(context.Response.Body, Encoding.UTF8))
                await writer.WriteAsync(data.ToString());
        }

        public async Task ProcessHeadersAsync(object data, IRequestContext context)
        {
            context.Response.AddHeader("Content-Type", "text/plain; charset=utf-8");
            await Task.Yield();
        }
    }
}
