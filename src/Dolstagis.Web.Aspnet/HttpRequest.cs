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
            this.AbsolutePath = new VirtualPath(Url.AbsolutePath);
            this.Path = new VirtualPath(innerRequest.ApplicationPath)
                .GetAppRelativePath(this.AbsolutePath, true);
            this.PathBase = new VirtualPath(innerRequest.ApplicationPath);
            this.Protocol = innerRequest.ServerVariables["SERVER_PROTOCOL"];
            this.IsSecure = innerRequest.IsSecureConnection;
            this.Query = new NameValueCollectionAdapter(innerRequest.Unvalidated.QueryString);
            this.Form = new NameValueCollectionAdapter(innerRequest.Unvalidated.Form);
            this.Headers = new RequestHeaders
                (new NameValueCollectionAdapter(innerRequest.Unvalidated.Headers));
        }

        public string Method { get; private set; }

        public VirtualPath AbsolutePath { get; private set; }

        public VirtualPath Path { get; private set; }

        public VirtualPath PathBase { get; private set; }

        public string Protocol { get; private set; }

        public bool IsSecure { get; private set; }

        public Uri Url { get; private set; }

        public IDictionary<string, string[]> Query { get; private set; }

        public IDictionary<string, string[]> Form { get; private set; }

        public RequestHeaders Headers { get; private set; }

    }
}
