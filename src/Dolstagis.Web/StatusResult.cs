using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Http;
using Dolstagis.Web.IoC;
using Dolstagis.Web.Views;

namespace Dolstagis.Web
{
    public class StatusResult : ResultBase
    {
        public StatusResult(Status status)
        {
            Status = status;
            MimeType = "text/html";
            Encoding = Encoding.UTF8;
        }

        protected override async Task SendBodyAsync(IRequestContext context)
        {
            VirtualPath viewPath = "~/errors/" + Status.Code;

            var registry = context.Container.Get<ViewRegistry>();
            var view = registry.GetView(viewPath);
            if (view == null) {
                viewPath = "~/errors/default";
                view = registry.GetView(viewPath);
            }
            if (view != null) {
                await view.Render(context.Response,
                    new ViewData() {
                        Encoding = Encoding,
                        Model = Status,
                        Path = viewPath,
                        Status = Status
                    }
                );
            }
            else {
                await DumpStatus(context);
            }
        }

        private async Task DumpStatus(IRequestContext context)
        {
            const string rn = "Dolstagis.Web.Errors.DefaultErrorPage.html";
            string template;

            using (var stream = this.GetType().Assembly.GetManifestResourceStream(rn))
            using (var reader = new StreamReader(stream))
            {
                template = await reader.ReadToEndAsync();
            }

            var html = template
                .Replace("{{code}}", Status.Code.ToString())
                .Replace("{{title}}", HttpUtility.HtmlEncode(Status.Description))
                .Replace("{{description}}", HttpUtility.HtmlEncode(Status.Message))
                .Replace("{{base}}", context.Request.PathBase.ToString());

            using (var writer = context.Response.GetStreamWriter()) {
                await writer.WriteAsync(html);
            }
        }
    }
}
