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

        public Task Process(object data, IRequestContext context)
        {
            var typedData = (T)data;
            ProcessHeaders(typedData, context);
            return Process(typedData, context);
        }

        private void ProcessHeaders(T typedData, IRequestContext context)
        {
            context.Response.Status = typedData.Status;
            foreach (var key in typedData.Headers.Keys) {
                context.Response.AddHeader(key, typedData.Headers[key]);
            }
            if (typedData.Encoding != null)
            {
                context.Response.AddHeader("Content-Encoding", typedData.Encoding.WebName);
            }
        }

        public abstract Task Process(T data, IRequestContext context);
    }
}
