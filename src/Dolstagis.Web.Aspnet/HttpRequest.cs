using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Dolstagis.Web.Http;

namespace Dolstagis.Web.Aspnet
{
    public class HttpRequest : IHttpRequest
    {
        public HttpRequest(HttpRequestBase innerRequest)
        {
            this.Url = innerRequest.Unvalidated.Url;
            this.Method = innerRequest.HttpMethod;
            this.Path = Url.AbsolutePath;
            this.AppRelativePath = Url.AbsolutePath
                .Substring(innerRequest.ApplicationPath.Length).TrimStart('/');
            this.Protocol = innerRequest.ServerVariables["SERVER_PROTOCOL"];
            this.IsSecure = innerRequest.IsSecureConnection;
            this.Query = innerRequest.Unvalidated.QueryString;
            this.Form = innerRequest.Unvalidated.Form;
        }

        public string Method { get; private set; }

        public string Path { get; private set; }

        public string AppRelativePath { get; private set; }

        public string Protocol { get; private set; }

        public bool IsSecure { get; private set; }

        public Uri Url { get; private set; }

        public NameValueCollection Query { get; private set; }

        public NameValueCollection Form { get; private set; }
    }
}
