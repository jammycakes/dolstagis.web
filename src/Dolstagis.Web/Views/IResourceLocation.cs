using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.Views
{
    public interface IResourceLocation
    {
        Stream OpenIfAvailable(string path, string appRoot);

    }
}
