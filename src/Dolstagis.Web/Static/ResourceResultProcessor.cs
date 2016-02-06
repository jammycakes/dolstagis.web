using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Dolstagis.Web.Lifecycle;

namespace Dolstagis.Web.Static
{
    public class ResourceResultProcessor : ResultProcessor<ResourceResult>
    {
        private static Regex reGetExtension = new Regex(@"\.[^\.]+$");
        private static string DefaultMimeType = "application/octet-stream";

        protected override void ProcessHeaders(ResourceResult typedData, IRequestContext context)
        {
            base.ProcessHeaders(typedData, context);
            var resource = typedData.Resource;
            if (resource == null) Status.NotFound.Throw();
            context.Response.Status = Status.OK;
            var ext = reGetExtension.Match(resource.Name);
            context.Response.AddHeader(
                "Content-Type", 
                ext.Success ? MimeTypeMap.GetMimeType(ext.Value) : "application/octet-stream"
            );
            context.Response.AddHeader("Last-Modified", resource.LastModified.ToString("R"));
            if (resource.Length.HasValue) {
                context.Response.AddHeader("Content-Length", resource.Length.Value.ToString());
            }
        }

        public override async Task ProcessBody(ResourceResult data, IRequestContext context)
        {
            var resource = data.Resource;
            using (var stream = resource.Open()) {
                await stream.CopyToAsync(context.Response.Body);
            }
        }
    }
}
