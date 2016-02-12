using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.Lifecycle.ResultProcessors
{
    public class TextResultProcessor : ResultProcessor
    {
        public override MatchResult Match(object data, IRequestContext context)
        {
            return MatchAccept(context, "text/plain", true);
        }

        public override async Task ProcessBodyAsync(object data, IRequestContext context)
        {
            using (var writer = new StreamWriter(context.Response.Body, Encoding.UTF8))
                await writer.WriteAsync(data.ToString());
        }

        public override async Task ProcessHeadersAsync(object data, IRequestContext context)
        {
            context.Response.Headers["Content-Type"] = new[] { "text/plain; charset=utf-8" };
            await Task.Yield();
        }
    }
}
