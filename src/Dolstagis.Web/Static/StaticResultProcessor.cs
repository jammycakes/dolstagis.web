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
        private IResourceLocator _locator;
        private IMimeTypes _mimeTypes;

        public StaticResultProcessor(IResourceLocator locator, IMimeTypes mimeTypes)
        {
            _locator = locator;
            _mimeTypes = mimeTypes;
        }

        public override async Task Process(StaticResult data, IRequestContext context)
        {
            var resource = _locator.GetResource(data.Path);
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
