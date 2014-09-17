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

            this.Body = environment[EnvironmentKeys.ResponseBody] as Stream;
            this.Headers = new ResponseHeaders
                (environment[EnvironmentKeys.ResponseHeaders] as IDictionary<string, string[]>);
        }

        public void AddHeader(string name, string value)
        {
            this.Headers.AddHeader(name, value);
        }

        public Stream Body { get; private set; }

        public ResponseHeaders Headers { get; private set; }

        public Status Status
        {
            get
            {
                object obj;
                if (this.environment.TryGetValue(EnvironmentKeys.ResponseStatusCode, out obj))
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
                this.environment[EnvironmentKeys.ResponseStatusCode] = status.Code;
                this.environment[EnvironmentKeys.ResponseReasonPhrase] = status.Description;
            }
        }

        public string Protocol
        {
            get
            {
                object result;
                if (this.environment.TryGetValue(EnvironmentKeys.ResponseProtocol, out result))
                {
                    return result as string;
                }
                else
                {
                    return this.environment[EnvironmentKeys.RequestProtocol] as string;
                }
            }
            set
            {
                this.environment[EnvironmentKeys.ResponseProtocol] = value;
            }
        }
    }
}
