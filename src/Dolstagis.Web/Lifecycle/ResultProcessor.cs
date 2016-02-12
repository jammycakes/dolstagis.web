using System;
using System.Threading.Tasks;
using Dolstagis.Web.Http;

namespace Dolstagis.Web.Lifecycle
{
    public abstract class ResultProcessor<T> : IResultProcessor where T: ResultBase
    {
        public MatchResult Match(object data, IRequestContext context)
        {
            if (data is T) return MatchResult.Exact;
            return MatchUntyped(data, context);
        }

        public virtual MatchResult MatchUntyped(object data, IRequestContext context)
        {
            return MatchResult.None;
        }


        public virtual async Task ProcessHeadersAsync(object data, IRequestContext context)
        {
            if (data is T) {
                await ProcessTypedHeadersAsync((T)data, context);
            }
            else {
                await ProcessUntypedHeadersAsync(data, context);
            }
        }

        public async Task ProcessBodyAsync(object data, IRequestContext context)
        {
            if (data is T) {
                await ProcessTypedBodyAsync((T)data, context);
            }
            else {
                await ProcessUntypedBodyAsync(data, context);
            }
        }

        protected virtual async Task ProcessTypedHeadersAsync(T typedData, IRequestContext context)
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

            await Task.Yield();
        }

        protected virtual async Task ProcessUntypedHeadersAsync(object data, IRequestContext context)
        {
            await Task.Yield();
        }

        protected abstract Task ProcessTypedBodyAsync(T data, IRequestContext context);

        protected async virtual Task ProcessUntypedBodyAsync(object data, IRequestContext context)
        {
            await Task.Yield();
        }
    }
}
