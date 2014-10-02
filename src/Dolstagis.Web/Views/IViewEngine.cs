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

        /// <summary>
        ///  Gets the view at a given virtual path.
        /// </summary>
        /// <param name="pathToView"></param>
        /// <param name="resolver"></param>
        /// <returns>
        ///  The view, if it exists, or null if it doesn't.
        /// </returns>
        IView GetView(VirtualPath pathToView, ResourceResolver resolver);
    }
}
