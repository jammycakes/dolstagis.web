﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Static;
using Dolstagis.Web.Views;

namespace Dolstagis.Web.Lifecycle
{
    public class ExceptionHandler : IExceptionHandler
    {
        private ViewRegistry _viewRegistry;

        public ExceptionHandler(ViewRegistry viewRegistry)
        {
            _viewRegistry = viewRegistry;
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

        private static async Task DumpException(IRequestContext context, Exception ex)
        {
            context.Response.AddHeader("Content-Type", "text/plain");
            context.Response.AddHeader("Content-Encoding", "utf-8");
            using (var writer = new StreamWriter(context.Response.ResponseStream, Encoding.UTF8))
            {
                await writer.WriteLineAsync(ex.ToString());
            }
        }

        private async Task HandleHttpStatusException(IRequestContext context, HttpStatusException ex)
        {
            context.Response.Status = ex.Status;

            var vPath = new VirtualPath("~/errors/" + ex.Status.Code);
            var view = _viewRegistry.GetView(vPath);
            if (view != null)
            {
                context.Response.AddHeader("Content-Type", "text/html");
                context.Response.AddHeader("Content-Encoding", "utf-8");
                await view.Render(context.Response.ResponseStream, new ViewResult(vPath.ToString(), ex));
            }
            else
            {
                await DumpException(context, ex);
            }
        }
    }
}
