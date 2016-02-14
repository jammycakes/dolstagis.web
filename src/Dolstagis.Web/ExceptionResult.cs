using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.IoC;

namespace Dolstagis.Web
{
    public class ExceptionResult : StatusResult
    {
        public Exception Exception { get; private set; }

        public ExceptionResult(Exception exception)
            : base(Status.InternalServerError)
        {
            Exception = exception;
        }

        protected override async Task SendBodyAsync(IRequestContext context)
        {
            var settings = context.Container.Get<ISettings>();
            if (!settings.Debug) {
                await base.SendBodyAsync(context);
            }
            else {
                await DumpException(context);
            }
        }

        private async Task DumpException(IRequestContext context)
        {
            await base.SendBodyAsync(context);
        }
    }
}
