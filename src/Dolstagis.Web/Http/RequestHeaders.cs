using System;
using System.Collections.Generic;
using System.Linq;

namespace Dolstagis.Web.Http
{
    public class RequestHeaders : HttpDictionary
    {
        private Lazy<IDictionary<string, Cookie>> _cookies;

        public RequestHeaders(IDictionary<string, string[]> inner)
            : base(inner)
        {
            _cookies = new Lazy<IDictionary<string, Cookie>>(GetCookies);
        }
        public IDictionary<string, Cookie> Cookies
        { get { return _cookies.Value;} }

        private IDictionary<string, Cookie> GetCookies()
        {
            var cookies = new Dictionary<string, Cookie>();
            string[] cookieHeaders;
            if (this.TryGetValue("Cookie", out cookieHeaders))
            {
                var data = from header in cookieHeaders
                           from str in header.Split(';')
                           let parts = str.Trim().Split(new char[] { '=' }, 2)
                           let key = parts[0]
                           let value = parts.Length == 2 ? HttpUtility.UrlDecode(parts[1]) : null
                           select new Cookie(key, value);
                foreach (var cookie in data) cookies[cookie.Name] = cookie;
            }
            else
            {
                cookies = new Dictionary<string, Cookie>();
            }
            return cookies;
        }

        public string ContentType
        {
            get { return this.GetValue("Content-Type"); }
            set { this.SetValue("Content-Type", value); }
        }
    }
}
