using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Dolstagis.Web;

namespace WebApp
{
    [Route("/")]
    public class Index : Handler
    {
        public async Task<string> Get()
        {
            return "Hello world";
        }
    }
}