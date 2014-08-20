using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.Http
{
    public class ResponseContext : IResponseContext
    {
        IResponse _innerResponse;

        public ResponseContext(IResponse innerResponse)
        {
            _innerResponse = innerResponse;
        }

        public void AddHeader(string name, string value)
        {
            _innerResponse.AddHeader(name, value);
        }

        public System.IO.Stream Body
        {
            get { return _innerResponse.Body; }
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
