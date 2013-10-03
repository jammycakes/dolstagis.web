using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Http;

namespace Dolstagis.Web.Views
{
    public interface IView
    {
        Task Render(IRequestContext context, object data);
    }
}
