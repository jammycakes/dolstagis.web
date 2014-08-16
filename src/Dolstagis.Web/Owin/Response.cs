using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Http;

namespace Dolstagis.Web.Owin
{
    public class Response : IResponse
    {
        private IDictionary<string, object> environment;

        public Response(IDictionary<string, object> environment)
        {
            this.environment = environment;
        }

        public void AddHeader(string name, string value)
        {
            throw new NotImplementedException();
        }

        public System.IO.Stream ResponseStream
        {
            get { throw new NotImplementedException(); }
        }

        public Status Status
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
