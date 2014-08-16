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
    public class HttpRequest : IRequest
    {
        public HttpRequest(HttpRequestBase innerRequest)
        {
            this.Url = innerRequest.Unvalidated.Url;
            this.Method = innerRequest.HttpMethod;
            this.Path = new VirtualPath(Url.AbsolutePath);
            this.AppRelativePath = new VirtualPath(innerRequest.ApplicationPath)
                .GetAppRelativePath(this.Path, true);
            this.Protocol = innerRequest.ServerVariables["SERVER_PROTOCOL"];
            this.IsSecure = innerRequest.IsSecureConnection;
            this.Query = new NameValueCollectionAdapter(innerRequest.Unvalidated.QueryString);
            this.Form = new NameValueCollectionAdapter(innerRequest.Unvalidated.Form);
            this.Headers = new NameValueCollectionAdapter(innerRequest.Unvalidated.Headers);
        }

        public string Method { get; private set; }

        public VirtualPath Path { get; private set; }

        public VirtualPath AppRelativePath { get; private set; }

        public string Protocol { get; private set; }

        public bool IsSecure { get; private set; }

        public Uri Url { get; private set; }

        public IDictionary<string, string[]> Query { get; private set; }

        public IDictionary<string, string[]> Form { get; private set; }


        public IDictionary<string, string[]> Headers { get; private set; }
    }
}
