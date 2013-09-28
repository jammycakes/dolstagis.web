using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web;

namespace Dolstagis.Tests.Web.TestModules.Handlers
{
    [Route("/")]
    public class RootHandler : Handler
    {
        public object Get()
        {
            return Status.OK;
        }
    }
}
