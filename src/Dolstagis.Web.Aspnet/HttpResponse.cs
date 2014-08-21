using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Dolstagis.Web.Http;

namespace Dolstagis.Web.Aspnet
{
    public class HttpResponse : IResponse
    {
        private HttpResponseBase _innerResponse;

        public HttpResponse(HttpResponseBase innerResponse)
        {
            _innerResponse = innerResponse;
            this.Headers = new ResponseHeaders
                (new NameValueCollectionAdapter(innerResponse.Headers));
        }

        public void AddHeader(string name, string value)
        {
            this.Headers.AddHeader(name, value);
        }

        public System.IO.Stream Body
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


        public ResponseHeaders Headers { get; private set; }

        public string Protocol
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
