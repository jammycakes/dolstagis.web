using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Dolstagis.Web.IoC;
using Dolstagis.Web.Views;

namespace Dolstagis.Web
{
    public class ViewResult : ResultBase
    {
        public VirtualPath Path { get; private set; }

        public object Model { get; private set; }

        public IDictionary<string, object> Data { get; private set; }

        public ViewResult(string path)
        {
            this.Path = new VirtualPath(path);
            this.Model = null;
            this.Data = new Dictionary<string, object>();
        }

        public ViewResult(string path, object model)
        {
            this.Path = new VirtualPath(path);
            this.Model = model;
            this.Data = new Dictionary<string, object>();
        }

        protected override async Task SendBodyAsync(IRequestContext context)
        {
            var registry = context.Container.Get<ViewRegistry>();
            var view = registry.GetView(Path);
            await view.Render(context.Response,
                new ViewData() {
                    Data = this.Data,
                    Encoding = this.Encoding,
                    Model = this.Model,
                    Path = this.Path,
                    Status = this.Status
                });
        }
    }
}
