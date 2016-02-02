using System;
using System.Threading.Tasks;

namespace Dolstagis.Web.Lifecycle
{
    public interface IExceptionHandler
    {
        Task HandleException(IRequestContext context, Exception ex);
    }
}
