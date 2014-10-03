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
        private ResourceResolver _resolver;

        private IResource _resource;

        public StaticResultProcessor(ResourceMapping[] mappings)
        {
            _resolver = new ResourceResolver(ResourceType.StaticFiles, mappings);
        }

        protected override void ProcessHeaders(StaticResult typedData, RequestContext context)
        {
            base.ProcessHeaders(typedData, context);
            _resource = _resolver.GetResource(typedData.Path);
            if (_resource == null) Status.NotFound.Throw();
            context.Response.Status = Status.OK;
            context.Response.AddHeader("Content-Type", MimeTypes.GetMimeType(_resource.Name));
            context.Response.AddHeader("Last-Modified", _resource.LastModified.ToString("R"));
            if (_resource.Length.HasValue)
            {
                context.Response.AddHeader("Content-Length", _resource.Length.Value.ToString());
            }
        }

        public override async Task ProcessBody(StaticResult data, RequestContext context)
        {
            using (var stream = _resource.Open()) {
                await stream.CopyToAsync(context.Response.Body);
            }
        }
    }
}
