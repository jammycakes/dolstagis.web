using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Http;
using Dolstagis.Web.Static;
using Dolstagis.Web.Views;

namespace Dolstagis.Web.Lifecycle
{
    public class ExceptionHandler : IExceptionHandler
    {
        private ViewRegistry _viewRegistry;
        private ISettings _settings;

        public ExceptionHandler(ViewRegistry viewRegistry, ISettings settings)
        {
            _viewRegistry = viewRegistry;
            _settings = settings;
        }

        public async Task HandleException(IRequestContext context, Exception ex)
        {
            if (ex is HttpStatusException)
            {
                await HandleHttpStatusException(context, (HttpStatusException)ex);
            }
            else
            {
                await HandleHttpStatusException(context, new HttpStatusException(ex));
            }
        }

        private async Task DumpException(IRequestContext context, HttpStatusException ex)
        {
            context.Response.Status = ex.Status;
            context.Response.AddHeader("Content-Type", "text/html");
            context.Response.AddHeader("Content-Encoding", "utf-8");

            const string rn = "Dolstagis.Web.Errors.DefaultErrorPage.html";
            string template;

            using (var stream = this.GetType().Assembly.GetManifestResourceStream(rn))
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                template = await reader.ReadToEndAsync();
            }

            var html = template
                .Replace("{{code}}", ex.Status.Code.ToString())
                .Replace("{{title}}", HttpUtility.HtmlEncode(ex.Status.Description))
                .Replace("{{description}}", HttpUtility.HtmlEncode(ex.Status.Message))
                .Replace("{{debug}}", _settings.Debug ? RenderDebugInfo(context, ex) : "");

            using (var writer = new StreamWriter(context.Response.ResponseStream, Encoding.UTF8))
            {
                await writer.WriteAsync(html);
            }
        }

        private string RenderDebugInfo(IRequestContext context, HttpStatusException ex)
        {
            return "";
        }

        private async Task HandleHttpStatusException(IRequestContext context, HttpStatusException ex)
        {
            HttpStatusException fault = null;

            var vPath = new VirtualPath("~/errors/" + ex.Status.Code);
            var view = _viewRegistry.GetView(vPath) ??
                _viewRegistry.GetView(new VirtualPath("~/errors/default"));
            if (view != null)
            {
                try
                {
                    context.Response.Status = ex.Status;
                    context.Response.AddHeader("Content-Type", "text/html");
                    context.Response.AddHeader("Content-Encoding", "utf-8");
                    await view.Render(context.Response.ResponseStream, new ViewResult(vPath.ToString(), ex));
                    return;
                }
                catch (Exception exRendering)
                {
                    fault = new HttpStatusException(new AggregateException(ex, exRendering));
                    context.Response.Status = Status.InternalServerError;
                }
            }
            else
            {
                await DumpException(context, fault ?? ex);
            }
        }
    }
}
