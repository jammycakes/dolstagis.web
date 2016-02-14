using System;
using System.IO;
using System.Threading.Tasks;
using Dolstagis.Web.Http;

namespace Dolstagis.Web
{
    public class ContentResult : ResultBase
    {
        public string Content { get; set; }

        public ContentResult(string content = "", string contentType = "text/plain") : base()
        {
            this.Content = content;
            this.MimeType = contentType;
            this.Encoding = System.Text.Encoding.UTF8;
        }

        protected override void SendHeaders(IRequestContext context)
        {
            context.Response.AddHeader("Content-Length", Encoding.GetByteCount(Content).ToString());
            base.SendHeaders(context);
        }

        protected override async Task SendBodyAsync(IRequestContext context)
        {
            if (!String.IsNullOrEmpty(Content))
                using (var writer = context.Response.GetStreamWriter())
                    await writer.WriteAsync(Content);
        }
    }
}
