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
    }
}
