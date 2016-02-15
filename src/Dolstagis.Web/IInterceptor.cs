using System;
using System.Threading.Tasks;

namespace Dolstagis.Web
{
    public interface IInterceptor
    {
        Task HandleException(IRequestContext context, Exception ex);
    }
}
