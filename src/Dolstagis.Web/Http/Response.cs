using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.Http
{
    public class Response : IResponse
    {
        IResponse _innerResponse;

        public Response(IResponse innerResponse)
        {
            _innerResponse = innerResponse;
        }

        public void AddHeader(string name, string value)
        {
            _innerResponse.AddHeader(name, value);
        }

        public System.IO.Stream ResponseStream
        {
            get { return _innerResponse.ResponseStream; }
        }

        public Status Status
        {
            get
            {
                return _innerResponse.Status;
            }
            set
            {
                _innerResponse.Status = value;
            }
        }

        public void AddCookie(Cookie cookie)
        {
            _innerResponse.AddHeader("Set-Cookie", cookie.ToHeaderString());
        }
    }
}
