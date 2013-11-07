using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Http;

namespace Dolstagis.Web.Lifecycle
{
    public interface IExceptionHandler
    {
        Task HandleException(IHttpContext context, Exception ex);
    }
}
