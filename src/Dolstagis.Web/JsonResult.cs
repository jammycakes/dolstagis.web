using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Dolstagis.Web
{
    public class JsonResult : ResultBase
    {
        public object Data { get; set; }

        public JsonResult(object data)
        {
            Data = data;
            MimeType = "application/json";
            Encoding = System.Text.Encoding.UTF8;
        }

        protected override async Task SendBodyAsync(IRequestContext context)
        {
            using (var textWriter = new StreamWriter(context.Response.Body, Encoding)) {
                var serializer = new JsonSerializer();
                await Task.Run(() => {
                    serializer.Serialize(textWriter, Data);
                });
            }
        }
    }
}