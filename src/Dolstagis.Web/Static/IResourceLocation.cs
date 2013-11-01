using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.Static
{
    public interface IResourceLocation
    {
        IResource GetResource(VirtualPath path);
    }
}
