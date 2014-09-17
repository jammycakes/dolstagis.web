using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.Auth
{
    public interface IRequirement
    {
        bool IsDenied(IRequestContext context);
    }
}
