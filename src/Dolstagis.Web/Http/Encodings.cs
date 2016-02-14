using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.Http
{
    public static class Encodings
    {
        private static IDictionary<string, Encoding> _dict
            = new Dictionary<string, Encoding>();

        static Encodings()
        {
            _dict = new Dictionary<string, Encoding>(StringComparer.OrdinalIgnoreCase);

            foreach (var info in Encoding.GetEncodings()) {
                var encoding = info.GetEncoding();
                _dict[encoding.WebName] = encoding;
                _dict[info.Name] = encoding;
            }
        }

        public static Encoding Lookup(string webName)
        {
            Encoding result;
            return _dict.TryGetValue(webName, out result) ? result : null;
        }
    }
}
