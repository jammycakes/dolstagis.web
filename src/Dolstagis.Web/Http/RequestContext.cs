using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.Http
{
    public class RequestContext : IRequestContext
    {
        private IRequest _innerRequest;
        private IDictionary<string, Cookie> _cookies;
        private string _sessionID;

        #region /* ====== IRequest implementation ====== */

        public RequestContext(IRequest innerRequest)
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
                        _cookies = new Dictionary<string, Cookie>();
                        foreach (var cookie in data) _cookies[cookie.Name] = cookie;
                    }
                    else
                    {
                        _cookies = new Dictionary<string, Cookie>();
                    }
                }
                return _cookies;
            }
        }

        private static RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

        public string SessionID
        {
            get
            {
                if (_sessionID == null)
                {
                    Cookie sessionCookie;
                    if (Cookies.TryGetValue(Constants.SessionKey, out sessionCookie))
                    {
                        _sessionID = sessionCookie.Value;
                    }
                    else
                    {
                        var bytes = new byte[32];
                        rng.GetBytes(bytes);
                        _sessionID = Convert.ToBase64String(bytes);
                    }
                }
                return _sessionID;
            }
        }
    }
}
