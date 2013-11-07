using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Lifecycle;

namespace Dolstagis.Web.Static
{
    public class StaticResultProcessor : ResultProcessor<StaticResult>
    {
        private IResourceResolver _resolver;
        private IMimeTypes _mimeTypes;

        public StaticResultProcessor(IResourceResolver resolver, IMimeTypes mimeTypes)
        {
            _resolver = resolver;
            _mimeTypes = mimeTypes;
        }

        public override async Task Process(StaticResult data, IHttpContext context)
        {
            var resource = _resolver.GetResource(data.Path);
            if (resource == null) Status.NotFound.Throw();
            context.Response.Status = Status.OK;
            context.Response.AddHeader("Content-Type", _mimeTypes.GetMimeType(resource.Name));
            context.Response.AddHeader("Last-Modified", resource.LastModified.ToString("R"));
            if (resource.Length.HasValue) {
                context.Response.AddHeader("Content-Length", resource.Length.Value.ToString());
            }
            using (var stream = resource.Open()) {
                await stream.CopyToAsync(context.Response.ResponseStream);
            }
        }
    }
}
