using System;
using System.Collections.Generic;
using System.Linq;

namespace Dolstagis.Web.Http
{
    public class RequestHeaders : HttpDictionary
    {
        private Lazy<IList<Option>> _accept;
        private Lazy<IList<Option>> _acceptEncoding;
        private Lazy<IList<Option>> _acceptLanguage;
        private Lazy<IDictionary<string, Cookie>> _cookies;

        public RequestHeaders(IDictionary<string, string[]> inner)
            : base(inner)
        {
            _accept = new Lazy<IList<Option>>(() => GetOptions("Accept"));
            _acceptEncoding = new Lazy<IList<Option>>(() => GetOptions("Accept-Encoding"));
            _acceptLanguage = new Lazy<IList<Option>>(() => GetOptions("Accept-Language"));
            _cookies = new Lazy<IDictionary<string, Cookie>>(GetCookies);
        }

        public IList<Option> Accept {  get { return _accept.Value; } }

        public IList<Option> AcceptEncoding { get { return _acceptEncoding.Value; } }

        public IList<Option> AcceptLanguage { get { return _acceptLanguage.Value; } }

        public string ContentType
        {
            get { return this.GetValue("Content-Type"); }
            set { this.SetValue("Content-Type", value); }
        }

        public IDictionary<string, Cookie> Cookies { get { return _cookies.Value;} }


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


        private IList<Option> GetOptions(string header)
        {
            string[] optHeaders;
            if (this.TryGetValue(header, out optHeaders)) {
                var result =
                    from h in optHeaders
                    from option in Option.ParseAll(h)
                    orderby option.Q descending
                    select option;
                return result.ToList();
            }
            else {
                return new List<Option>();
            }
        }
    }
}
