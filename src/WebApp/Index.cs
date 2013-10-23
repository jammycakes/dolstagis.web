using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Dolstagis.Web;
using Dolstagis.Web.Views;

namespace WebApp
{
    [Route("/")]
    public class Index : Handler
    {
        public object Get()
        {
            return View("~/hello.nustache", new { Message = "Hello from Nustache" });
        }
    }
}