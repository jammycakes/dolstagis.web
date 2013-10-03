using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.Lifecycle
{
    public class ExceptionHandler : IExceptionHandler
    {
        public async Task HandleException(IRequestContext context, Exception ex)
        {
            context.Response.Status = Status.InternalServerError;
            context.Response.AddHeader("Content-Type", "text/plain");
            context.Response.AddHeader("Content-Encoding", "utf-8");
            using (var writer = new StreamWriter(context.Response.ResponseStream, Encoding.UTF8)) {
                await writer.WriteLineAsync(ex.ToString());
            }
        }
    }
}
