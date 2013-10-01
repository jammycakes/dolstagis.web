using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.Views.Static
{
    public interface IResourceLocation
    {
        IResource Get(string path, string appRoot);
    }
}
