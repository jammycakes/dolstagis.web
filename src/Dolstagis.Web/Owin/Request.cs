using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Http;

namespace Dolstagis.Web.Owin
{
    public class Request : IRequest
    {
        private IDictionary<string, object> environment;

        public Request(IDictionary<string, object> environment)
        {
            this.environment = environment;
        }

        public string Method
        {
            get { throw new NotImplementedException(); }
        }

        public VirtualPath AbsolutePath
        {
            get { throw new NotImplementedException(); }
        }

        public VirtualPath Path
        {
            get { throw new NotImplementedException(); }
        }

        public string Protocol
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsSecure
        {
            get { throw new NotImplementedException(); }
        }

        public Uri Url
        {
            get { throw new NotImplementedException(); }
        }

        public IDictionary<string, string[]> Query
        {
            get { throw new NotImplementedException(); }
        }

        public IDictionary<string, string[]> Form
        {
            get { throw new NotImplementedException(); }
        }

        public IDictionary<string, string[]> Headers
        {
            get { throw new NotImplementedException(); }
        }
    }
}
