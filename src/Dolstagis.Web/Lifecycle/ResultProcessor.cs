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

        public Task Process(object data, IHttpContext context)
        {
            var typedData = (T)data;
            ProcessHeaders(typedData, context);
            return Process(typedData, context);
        }

        protected virtual void ProcessHeaders(T typedData, IHttpContext context)
        {
            // Location: header should be absolute per RFC 2616 para 14.30. Enforce this.

            string location;
            if (typedData.Headers.TryGetValue("Location", out location))
            {
                Uri u;
                if (!Uri.TryCreate(location, UriKind.Absolute, out u))
                {
                    u = context.Request.GetAbsoluteUrl(new VirtualPath(location));
                    typedData.Headers["Location"] = u.ToString();
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

        public abstract Task Process(T data, IHttpContext context);
    }
}
