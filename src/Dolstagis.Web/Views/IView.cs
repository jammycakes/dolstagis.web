using System.IO;
using System.Threading.Tasks;

namespace Dolstagis.Web.Views
{
    public interface IView
    {
        Task Render(Stream stream, ViewData data);
    }
}
