using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Http;

namespace Dolstagis.Web
{
    public interface IRequestContext
    {
        IRequest Request { get; }

        IResponse Response { get; }
    }
}
