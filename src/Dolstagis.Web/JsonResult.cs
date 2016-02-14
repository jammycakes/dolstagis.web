using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Dolstagis.Web
{
    public class JsonResult : ResultBase
    {
        public object Model { get; set; }

        public JsonResult(object model)
        {
            Model = model;
            MimeType = "application/json";
            Encoding = System.Text.Encoding.UTF8;
        }

        protected override Task SendBodyAsync(IRequestContext context)
        {
            using (var textWriter = new StreamWriter(context.Response.Body, Encoding)) {
                var serializer = new JsonSerializer();
                serializer.Serialize(textWriter, Model);
            }
            return Task.FromResult(0);
        }
    }
}