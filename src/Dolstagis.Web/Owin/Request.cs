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
        public Request(IDictionary<string, object> environment)
        {
            this.Environment = environment;

            /* From the OWIN 1.0 specification section 3.2.1 */

            // TODO: owin.RequestBody
            this.Headers = environment["owin.RequestHeaders"] as IDictionary<string, string[]>;
            this.Method = environment["owin.RequestMethod"] as string;
            string pathBase = environment["owin.RequestPathBase"] as string;
            if (String.IsNullOrEmpty(pathBase)) pathBase = "/";
            this.PathBase = new VirtualPath(pathBase);
            string path = (environment["owin.RequestPath"] as string).TrimStart('/');
            this.Path = new VirtualPath(path);
            this.AbsolutePath = this.PathBase.Append(this.Path);
            this.Protocol = environment["owin.RequestProtocol"] as string;
            string queryString = environment["owin.RequestQueryString"] as string;
            this.Query = ParseQueryString(queryString);
            this.Scheme = environment["owin.RequestScheme"] as string;

            /* Computed */

            this.Host = this.Headers["Host"].First();
            string url = this.Scheme + "://" + this.Host + this.AbsolutePath +
                (String.IsNullOrEmpty(queryString) ? String.Empty : queryString);
            this.Url = new Uri(url);
            this.IsSecure = String.Compare("https", this.Scheme, true) == 0;

            // TODO: Form
        }


        private static IDictionary<string, string[]> ParseQueryString(string query)
        {
            var parsed = HttpUtility.ParseQueryString(query);
            var result = new Dictionary<string, string[]>(parsed.Count);
            foreach (string key in parsed.Keys) {
                result[key] = parsed.GetValues(key);
            }
            return result;
        }

        public IDictionary<string, object> Environment { get; private set; }

        public IDictionary<string, string[]> Headers { get; private set; }

        public string Method { get; private set; }

        public VirtualPath PathBase { get; private set; }

        public VirtualPath Path { get; private set; }

        public VirtualPath AbsolutePath { get; private set; }

        public string Protocol { get; private set; }

        public IDictionary<string, string[]> Query { get; private set; }

        public string Scheme { get; private set; }

        public string Host { get; private set; }

        public Uri Url { get; private set; }

        public bool IsSecure { get; private set; }

        public IDictionary<string, string[]> Form { get; private set; }
    }
}
