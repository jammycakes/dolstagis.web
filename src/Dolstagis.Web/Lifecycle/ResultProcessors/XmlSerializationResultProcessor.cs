using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Dolstagis.Web;
using Dolstagis.Web.Lifecycle;

namespace Dolstagis.Web.Lifecycle.ResultProcessors
{
    public class XmlSerializationResultProcessor : ResultProcessor
    {
        private static Regex reIsXml = new Regex(@"^(text|application)/(.*\+)xml$");

        private string _mimeType;


        public override MatchResult Match(object data, IRequestContext context)
        {
            var result = MatchAccept(context, reIsXml, false);
            _mimeType = result.Value ?? "application/xml";
            return result;
        }

        public override async Task ProcessBodyAsync(object data, IRequestContext context)
        {
            await Task.Run(() => {
                var serializer = new XmlSerializer(data.GetType());
                using (var writer = new StreamWriter(context.Response.Body, Encoding.UTF8))
                    serializer.Serialize(writer, data);
            });
        }

        public override async Task ProcessHeadersAsync(object data, IRequestContext context)
        {
            context.Response.Headers["Content-Type"] = new[] { _mimeType + "; charset=utf-8" };
            await Task.Yield();
        }
    }
}
