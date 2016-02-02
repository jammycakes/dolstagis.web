﻿using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Dolstagis.Web.Lifecycle.ResultProcessors
{
    public class JsonResultProcessor : ResultProcessor<JsonResult>
    {
        public static readonly JsonResultProcessor Instance = new JsonResultProcessor();

        private JsonResultProcessor()
        { }

        public override async Task ProcessBody(JsonResult data, IRequestContext context)
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
