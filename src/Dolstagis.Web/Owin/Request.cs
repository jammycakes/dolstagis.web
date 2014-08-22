using System;
using System.Collections.Generic;
using System.IO;
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

            this.Body = environment[EnvironmentKeys.RequestBody] as Stream;
            this.Headers = new RequestHeaders
                (environment[EnvironmentKeys.RequestHeaders] as IDictionary<string, string[]>);
            this.Method = environment[EnvironmentKeys.RequestMethod] as string;
            string pathBase = environment[EnvironmentKeys.RequestPathBase] as string;
            if (String.IsNullOrEmpty(pathBase)) pathBase = "/";
            this.PathBase = new VirtualPath(pathBase);
            string path = (environment[EnvironmentKeys.RequestPath] as string).TrimStart('/');
            this.Path = new VirtualPath(path);
            this.AbsolutePath = this.PathBase.Append(this.Path);
            this.Protocol = environment[EnvironmentKeys.RequestProtocol] as string;
            string queryString = environment[EnvironmentKeys.RequestQueryString] as string;
            this.Query = ParseQueryString(queryString);
            this.Scheme = environment[EnvironmentKeys.RequestScheme] as string;

            /* Computed */

            this.Host = this.Headers["Host"].First();
            string url = this.Scheme + "://" + this.Host + this.AbsolutePath +
                (String.IsNullOrEmpty(queryString) ? String.Empty : queryString);
            this.Url = new Uri(url);
            this.IsSecure = String.Compare("https", this.Scheme, true) == 0;

            // TODO: Form

            this.Form = ParseForm(this.Body, this.Headers.ContentType, Encoding.UTF8);
        }


        private static IDictionary<string, string[]> ParseForm
            (Stream stream, string contentType, Encoding encoding)
        {
            if (String.IsNullOrEmpty(contentType))
            {
                return new Dictionary<string, string[]>();
            }

            var mimeType = contentType.Split(';').First();
            if (mimeType.Equals("application/x-www-form-urlencoded", StringComparison.OrdinalIgnoreCase))
            {
                var reader = new StreamReader(stream);
                string form = reader.ReadToEnd();
                return ParseQueryString(form);
            }

            return new Dictionary<string, string[]>();
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


        public Stream Body { get; private set; }

        public IDictionary<string, object> Environment { get; private set; }

        public RequestHeaders Headers { get; private set; }

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
