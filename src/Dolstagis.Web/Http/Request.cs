using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.Http
{
    public class Request : IRequest
    {
        private IRequest _innerRequest;
        private IDictionary<string, Cookie> _cookies;

        #region /* ====== IRequest implementation ====== */

        public Request(IRequest innerRequest)
        {
            _innerRequest = innerRequest;
        }

        public string Method
        {
            get { return _innerRequest.Method; }
        }

        public VirtualPath Path
        {
            get { return _innerRequest.Path; }
        }

        public VirtualPath AppRelativePath
        {
            get { return _innerRequest.AppRelativePath; }
        }

        public string Protocol
        {
            get { return _innerRequest.Protocol; }
        }

        public bool IsSecure
        {
            get { return _innerRequest.IsSecure; }
        }

        public Uri Url
        {
            get { return _innerRequest.Url; }
        }

        public System.Collections.Specialized.NameValueCollection Query
        {
            get { return _innerRequest.Query; }
        }

        public System.Collections.Specialized.NameValueCollection Form
        {
            get { return _innerRequest.Form; }
        }

        public System.Collections.Specialized.NameValueCollection Headers
        {
            get { return _innerRequest.Headers; }
        }

        #endregion


        public IDictionary<string, Cookie> Cookies
        {
            get
            {
                if (_cookies == null)
                {
                    var cookieHeaders = Headers.GetValues("Cookie");
                    if (cookieHeaders != null)
                    {
                        var data = from header in cookieHeaders
                                   from str in header.Split(';')
                                   let parts = str.Trim().Split(new char[] { '=' }, 2)
                                   let key = parts[0]
                                   let value = parts.Length == 2 ? HttpUtility.UrlDecode(parts[1]) : null
                                   select new Cookie(key, value);
                        _cookies = data.ToDictionary(k => k.Name);
                    }
                    else
                    {
                        _cookies = new Dictionary<string, Cookie>();
                    }
                }
                return _cookies;
            }
        }
    }
}
