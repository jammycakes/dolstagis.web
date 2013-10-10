using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Static;

namespace Dolstagis.Web.Views
{
    public interface IViewEngine
    {
        IEnumerable<string> Extensions { get; }

        IView GetView(VirtualPath pathToView, IResourceResolver resolver);
    }
}
