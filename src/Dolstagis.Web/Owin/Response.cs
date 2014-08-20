using System;
using System.Collections.Generic;
using System.IO;
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

            this.Body = environment["owin.ResponseBody"] as Stream;
            this.Headers = new ResponseHeaders
                (environment["owin.ResponseHeaders"] as IDictionary<string, string[]>);
        }

        public void AddHeader(string name, string value)
        {
            string[] values;
            if (Headers.TryGetValue(name, out values))
            {
                Headers[name] = values.Concat(new string[] { value }).ToArray();
            }
            else
            {
                Headers[name] = new string[] { value };
            }
        }

        public Stream Body { get; private set; }

        public ResponseHeaders Headers { get; private set; }

        public Status Status
        {
            get
            {
                object obj;
                if (this.environment.TryGetValue("owin.ResponseStatusCode", out obj))
                {
                    return Status.ByCode((int)obj);
                }
                else
                {
                    return Status.OK;
                }
            }
            set
            {
                var status = value ?? Status.OK;
                this.environment["owin.ResponseStatusCode"] = status.Code;
                this.environment["owin.ResponseReasonPhrase"] = status.Description;
            }
        }
    }
}
