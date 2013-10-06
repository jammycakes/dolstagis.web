using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.Views
{
    public interface IViewEngine
    {
        IEnumerable<string> Extensions { get; }

        IView GetView(VirtualPath pathToView);
    }
}
