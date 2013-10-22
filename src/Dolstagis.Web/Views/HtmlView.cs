using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Http;

namespace Dolstagis.Web.Views
{
    public class HtmlView : IView
    {
        public async Task Render(IRequestContext context, ViewResult data)
        {
            var response = context.Response;
            if (data == null) {
                response.Status = Status.NoContent;
            }
            else {
                response.Status = Status.OK;
                response.AddHeader("Content-Type", "text/html");
                response.AddHeader("Content-Encoding", "utf8");
                using (var writer = new StreamWriter(response.ResponseStream)) {
                    await writer.WriteAsync(data.ToString());
                }
            }
        }
    }
}
