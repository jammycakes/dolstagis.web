using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Dolstagis.Web.Lifecycle.ResultProcessors
{
    public class JsonResultProcessor : ResultProcessor<JsonResult>
    {
        public static readonly JsonResultProcessor Instance = new JsonResultProcessor();

        private JsonResultProcessor()
        { }

        private static readonly Regex reIsJson = new Regex(@"^application/(.*\+)?json$");

        public override MatchResult MatchUntyped(object data, IRequestContext context)
        {
            var accept = context.Request.Headers.Accept;
            if (!accept.Any()) {
                return MatchResult.None;
            }

            return (
                from opt in accept
                let isExact = reIsJson.IsMatch(opt.Value)
                let isInexact = opt.Value == "*/*"
                let result = new MatchResult(
                    isExact ? Lifecycle.Match.Exact : Lifecycle.Match.Fallback,
                    opt.Q
                )
                orderby result.Match descending, result.Q descending
                select result
            ).FirstOrDefault() ?? MatchResult.None;
        }

        protected override async Task ProcessTypedBodyAsync(JsonResult data, IRequestContext context)
        {
            await ProcessJsonAsync(data.Data, context, data.Encoding);
        }

        protected override async Task ProcessUntypedBodyAsync(object data, IRequestContext context)
        {
            await ProcessJsonAsync(data, context, Encoding.UTF8);
        }

        protected async Task ProcessJsonAsync(object data, IRequestContext context, Encoding encoding)
        {
            using (var textWriter = new StreamWriter(context.Response.Body, encoding))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(textWriter, data);
            }
            await Task.Yield();
        }
    }
}
