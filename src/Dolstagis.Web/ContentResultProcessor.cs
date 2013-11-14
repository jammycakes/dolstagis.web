using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Lifecycle;

namespace Dolstagis.Web
{
    public class ContentResultProcessor : ResultProcessor<ContentResult>
    {
        public override async Task Process(ContentResult data, IHttpContext context)
        {
            if (!String.IsNullOrEmpty(data.Content))
            {
                using (var writer = new StreamWriter(context.Response.ResponseStream, data.Encoding))
                {
                    await writer.WriteAsync(data.Content);
                }
            }
        }
    }
}