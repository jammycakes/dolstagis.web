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
        public static readonly ContentResultProcessor Instance = new ContentResultProcessor();

        private ContentResultProcessor()
        { }

        public override async Task ProcessBody(ContentResult data, RequestContext context)
        {
            if (!String.IsNullOrEmpty(data.Content))
            {
                using (var writer = new StreamWriter(context.Response.Body, data.Encoding))
                {
                    await writer.WriteAsync(data.Content);
                }
            }
        }
    }
}