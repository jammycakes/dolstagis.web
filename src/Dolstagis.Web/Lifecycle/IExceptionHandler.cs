using System;
using System.Threading.Tasks;

namespace Dolstagis.Web.Lifecycle
{
    public interface IExceptionHandler
    {
        Task HandleException(RequestContext context, Exception ex);
    }
}
