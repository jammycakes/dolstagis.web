using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Dolstagis.Web.Http;

namespace Dolstagis.Web.Aspnet
{
    public class HttpResponse : IHttpResponse
    {
        private HttpResponseBase _innerResponse;

        public HttpResponse(HttpResponseBase innerResponse)
        {
            _innerResponse = innerResponse;
        }

        public void AddHeader(string name, string value)
        {
            _innerResponse.AppendHeader(name, value);
        }

        public System.IO.Stream ResponseStream
        {
            get { return _innerResponse.OutputStream; }
        }

        public Status Status
        {
            get
            {
                return Status.ByCode(_innerResponse.StatusCode);
            }
            set
            {
                _innerResponse.StatusCode = value.Code;
                _innerResponse.StatusDescription = value.Description;
            }
        }
    }
}
