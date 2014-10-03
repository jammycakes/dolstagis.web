using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Http;

namespace Dolstagis.Web.Lifecycle
{
    public abstract class ResultProcessor<T> : IResultProcessor where T: ResultBase
    {
        public bool CanProcess(object data)
        {
            return (data is T);
        }

        public Task Process(object data, RequestContext context)
        {
            var typedData = (T)data;
            ProcessHeaders(typedData, context);
            return ProcessBody(typedData, context);
        }

        protected virtual void ProcessHeaders(T typedData, RequestContext context)
        {
            // Location: header should be absolute per RFC 2616 para 14.30. Enforce this.

            string location;
            if (typedData.Headers.TryGetValue("Location", out location))
            {
                Uri u;
                if (!Uri.TryCreate(location, UriKind.Absolute, out u))
                {
                    var parts = location.Split(new char[] { '?' }, 2);
                    if (parts.Length == 2)
                    {
                        u = context.Request.GetAbsoluteUrl(new VirtualPath(parts[0]));
                        typedData.Headers["Location"] = u.ToString() + "?" + parts[1];
                    }
                    else
                    {
                        u = context.Request.GetAbsoluteUrl(new VirtualPath(location));
                        typedData.Headers["Location"] = u.ToString();
                    }
                }
            }

            typedData.Headers.Remove("Content-Encoding");
            context.Response.Status = typedData.Status;
            foreach (var key in typedData.Headers.Keys) {
                context.Response.AddHeader(key, typedData.Headers[key]);
            }
            if (typedData.Encoding != null)
            {
                context.Response.AddHeader("Content-Encoding", typedData.Encoding.WebName);
            }
        }

        public abstract Task ProcessBody(T data, RequestContext context);
    }
}
