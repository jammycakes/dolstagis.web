using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web;
using Dolstagis.Web.Aspnet;

namespace $rootnamespace$
{
    [Route("/")]
    public class Index : Handler
    {
        public object Get()
        {
            return this.Content("Hello from Dolstagis", "text/plain");
        }
    }
}
