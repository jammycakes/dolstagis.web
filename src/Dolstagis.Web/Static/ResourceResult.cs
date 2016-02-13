using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Dolstagis.Web.Static
{
    public class ResourceResult : ResultBase
    {
        private static Regex reGetExtension = new Regex(@"\.[^\.]+$");
        private const string DefaultMimeType = "application/octet-stream";

        public IResource Resource { get; private set; }

        public ResourceResult(IResource resource)
        {
            Resource = resource;
        }

        protected override void SendHeaders(IRequestContext context)
        {
            base.SendHeaders(context);
            if (Resource == null) Status.NotFound.Throw();
            context.Response.Status = Status.OK;
            var ext = reGetExtension.Match(Resource.Name);
            context.Response.AddHeader(
                "Content-Type",
                ext.Success ? MimeTypeMap.GetMimeType(ext.Value) : DefaultMimeType
            );
            context.Response.AddHeader("Last-Modified", Resource.LastModified.ToString("R"));
            if (Resource.Length.HasValue) {
                context.Response.AddHeader("Content-Length", Resource.Length.Value.ToString());
            }
        }

        protected override async Task SendBodyAsync(IRequestContext context)
        {
            using (var stream = Resource.Open()) {
                await stream.CopyToAsync(context.Response.Body);
            }
        }
    }
}
