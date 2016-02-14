using System.IO;
using System.Threading.Tasks;
using Dolstagis.Web.Http;

namespace Dolstagis.Web.Views
{
    public interface IView
    {
        Task Render(IResponse response, ViewData data);
    }
}
