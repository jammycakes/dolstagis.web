using System.IO;
using System.Threading.Tasks;
using Dolstagis.Web.Lifecycle;
using Newtonsoft.Json;

namespace Dolstagis.Web.Lifecycle.ResultProcessors
{
    public class JsonResultProcessor : ResultProcessor<JsonResult>
    {
        public static readonly JsonResultProcessor Instance = new JsonResultProcessor();

        private JsonResultProcessor()
        { }

        public override async Task ProcessBody(JsonResult data, RequestContext context)
        {
            using (var textWriter = new StreamWriter(context.Response.Body, data.Encoding))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(textWriter, data);
            }
            await Task.Yield();
        }
    }
}
