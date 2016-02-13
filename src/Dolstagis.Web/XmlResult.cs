using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Dolstagis.Web
{
    public class XmlResult : ResultBase
    {
        public object Model { get; set; }

        public XmlSerializer Serializer { get; set; }

        public XmlResult(object model)
        {
            Model = model;
            MimeType = "application/xml";
            Encoding = System.Text.Encoding.UTF8;
        }

        protected override async Task SendBodyAsync(IRequestContext context)
        {
            var ser = Serializer ?? new XmlSerializer(Model.GetType());
            using (var writer = new StreamWriter
                (context.Response.Body, Encoding ?? Encoding.UTF8)) {
                await Task.Run(() => ser.Serialize(writer, Model));
            }
        }
    }
}
