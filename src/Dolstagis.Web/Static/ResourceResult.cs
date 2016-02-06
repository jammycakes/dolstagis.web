using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.Static
{
    public class ResourceResult : ResultBase
    {
        public IResource Resource { get; private set; }

        public ResourceResult(IResource resource)
        {
            Resource = resource;
        }
    }
}
