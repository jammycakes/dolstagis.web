using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Http;

namespace Dolstagis.Web.Views
{
    public interface IView
    {
        Task Render(Stream stream, ViewResult data);
    }
}
