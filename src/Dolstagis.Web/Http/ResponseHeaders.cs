using System.Collections.Generic;
using System.Linq;

namespace Dolstagis.Web.Http
{
    public class ResponseHeaders : HttpDictionary
    {
        public ResponseHeaders(IDictionary<string, string[]> inner)
            : base(inner)
        { }

        public void AddHeader(string name, string value)
        {
            string[] values;
            if (this.TryGetValue(name, out values))
            {
                this[name] = values.Concat(new string[] { value }).ToArray();
            }
            else
            {
                this[name] = new string[] { value };
            }
        }


        public void AddCookie(Cookie cookie)
        {
            this.AddHeader("Set-Cookie", cookie.ToHeaderString());
        }
    }
}
