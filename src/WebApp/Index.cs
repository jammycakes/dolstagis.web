using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dolstagis.Web;

namespace WebApp
{
    [Route("/")]
    public class Index : Handler
    {
        public object Get()
        {
            return Status.OK;
        }
    }
}