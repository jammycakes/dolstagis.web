using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Lifecycle;
using Newtonsoft.Json;

namespace Dolstagis.Web
{
    public class JsonResultProcessor : ResultProcessor<JsonResult>
    {
        public override async Task Process(JsonResult data, IRequestContext context)
        {
            using (var textWriter = new StreamWriter(context.Response.ResponseStream, data.Encoding))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(textWriter, data);
            }
        }
    }
}
