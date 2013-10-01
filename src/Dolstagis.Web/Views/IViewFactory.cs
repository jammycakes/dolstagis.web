using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Views;

namespace Dolstagis.Web.Views
{
    public interface IViewFactory
    {
        /// <summary>
        ///  Creates a view for this result if possible.
        /// </summary>
        /// <param name="data">
        ///  The data which was returned from the handler action.
        /// </param>
        /// <returns>
        ///  An <see cref="IView" /> instance, or null if none could be created.
        /// </returns>

        IView CreateView(object data);
    }
}
