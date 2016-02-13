using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Http;

namespace Dolstagis.Web
{
    public abstract class ResultBase : IResult
    {
        protected string GetHeader(string key)
        {
            string result;
            return Headers.TryGetValue(key, out result) ? result : null;
        }

        protected void SetHeader(string key, string value)
        {
            if (key == null)
            {
                if (Headers.ContainsKey(key))
                {
                    Headers.Remove(key);
                }
            }
            else
            {
                Headers[key] = value;
            }
        }

        public Status Status { get; set; }

        public Encoding Encoding { get; set; }

        public string MimeType { get; set; }

        public IDictionary<string, string> Headers { get; private set; }

        public ResultBase()
        {
            Headers = new Dictionary<string, string>();
            Status = Status.OK;
            Encoding = Encoding.UTF8;
        }

        public virtual async Task RenderAsync(IRequestContext context)
        {
            SendHeaders(context);
            await SendBodyAsync(context);
        }

        protected virtual void SendHeaders(IRequestContext context)
        {
            // Location: header should be absolute per RFC 2616 para 14.30. Enforce this.

            string location;
            if (Headers.TryGetValue("Location", out location)) {
                Uri u;
                if (!Uri.TryCreate(location, UriKind.Absolute, out u)) {
                    var parts = location.Split(new char[] { '?' }, 2);
                    if (parts.Length == 2) {
                        u = context.Request.GetAbsoluteUrl(new VirtualPath(parts[0]));
                        Headers["Location"] = u.ToString() + "?" + parts[1];
                    }
                    else {
                        u = context.Request.GetAbsoluteUrl(new VirtualPath(location));
                        Headers["Location"] = u.ToString();
                    }
                }
            }

            context.Response.Status = Status;
            foreach (var key in Headers.Keys) {
                context.Response.AddHeader(key, Headers[key]);
            }

            if (MimeType != null) {
                context.Response.AddHeader("Content-Type",
                    MimeType + (Encoding != null
                        ? "; charset=" + Encoding.WebName
                        : String.Empty)
                );
            }
        }

        protected abstract Task SendBodyAsync(IRequestContext context);
    }
}
