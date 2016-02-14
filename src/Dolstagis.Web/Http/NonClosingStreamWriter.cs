using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.Http
{
    public class NonClosingStreamWriter : StreamWriter
    {
        public NonClosingStreamWriter(Stream stream, Encoding encoding) : base(stream, encoding)
        {
        }

        public override void Close()
        {
            Flush();
        }

        protected override void Dispose(bool disposing)
        {
            Flush();
        }
    }
}
