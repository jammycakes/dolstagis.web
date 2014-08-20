using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.Http
{
    public class ResponseHeaders : HttpDictionary
    {
        public ResponseHeaders(IDictionary<string, string[]> inner)
            : base(inner)
        { }
    }
}
