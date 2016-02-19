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

            var resolver = context.Container.Get<IViewResolver>();
            var view = resolver.GetView(viewPath);
            if (view == null) {
                viewPath = "~/errors/default";
                view = resolver.GetView(viewPath);
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

        private static readonly string template;

        static StatusResult()
        {
            const string rn = "Dolstagis.Web._dolstagis.DefaultErrorPage.html";
            using (var stream = typeof(StatusResult).Assembly.GetManifestResourceStream(rn))
            using (var reader = new StreamReader(stream))
            {
                template = reader.ReadToEnd();
            }
        }

        protected string GetTemplate(IRequestContext context)
        {
            return template
                .Replace("{{code}}", Status.Code.ToString())
                .Replace("{{title}}", HttpUtility.HtmlEncode(Status.Description))
                .Replace("{{description}}", HttpUtility.HtmlEncode(Status.Message))
                .Replace("{{base}}", context.Request.PathBase.ToString());
        }


        private async Task DumpStatus(IRequestContext context)
        {
            var html = GetTemplate(context)
                .Replace("{{exception}}", String.Empty);

            using (var writer = context.Response.GetStreamWriter()) {
                await writer.WriteAsync(html);
            }
        }
    }
}
