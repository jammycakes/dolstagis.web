using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.Http
{
    public static class ResponseExtensions
    {
        public static StreamWriter GetStreamWriter(this IResponse response)
        {
            return new StreamWriter(
                response.Body,
                response.Headers.Encoding ?? Encoding.UTF8,
                1024,
                true);
        }
    }
}
