using System;
using System.IO;
using System.Threading.Tasks;

namespace Dolstagis.Web
{
    public class RedirectResult : ResultBase
    {
        public string Location
        {
            get { return GetHeader("Location"); }
            set { SetHeader("Location", value); }
        }

        public RedirectResult(string location, Status status = null)
        {
            this.Location = location;
            this.Status = status ?? Status.TemporaryRedirect;
        }

        protected override async Task SendBodyAsync(IRequestContext context)
        {
            var message = String.Format(Status.Message, Location);

            using (var writer = new StreamWriter(context.Response.Body)) {
                await writer.WriteLineAsync("<!DOCTYPE html>");
                await writer.WriteLineAsync("<html>");
                await writer.WriteLineAsync("<body>");
                await writer.WriteLineAsync(String.Format("<p>{0}</p>", message));
                await writer.WriteLineAsync("</body>");
                await writer.WriteLineAsync("</html>");
            }
        }
    }
}
