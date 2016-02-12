using System;
using System.IO;
using System.Threading.Tasks;

namespace Dolstagis.Web.Lifecycle.ResultProcessors
{
    public class ContentResultProcessor : ResultProcessor<ContentResult>
    {
        public static readonly ContentResultProcessor Instance = new ContentResultProcessor();

        private ContentResultProcessor()
        { }

        protected override async Task ProcessTypedBodyAsync(ContentResult data, IRequestContext context)
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