using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.Features
{
    public interface IRouteExpression
    {
        IRouteFromExpression From(VirtualPath path);

        // void Controller<TController>();
    }
}
