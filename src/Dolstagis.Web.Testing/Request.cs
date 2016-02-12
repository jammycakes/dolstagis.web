using System;
using System.Collections.Generic;
using System.Linq;
using Dolstagis.Web.Http;

namespace Dolstagis.Web.Testing
{
    public class Request : IRequest
    {
        private string _method;
        private VirtualPath _pathBase;
        private VirtualPath _absolutePath;
        private VirtualPath _relativePath;
        private IDictionary<string, string[]> _form
            = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase);
        private IDictionary<string, string[]> _query
            = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase);
        private RequestHeaders _headers = new RequestHeaders
            (new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase));
        private bool _isSecure = false;
        private string _protocol = "HTTP/1.1";
        private Uri _url;

        private Request(string method, Uri url)
        {
            _url = url;
            _method = method;

            var parsed = HttpUtility.ParseQueryString(url.Query);
            _query = new Dictionary<string, string[]>(parsed.Count);
            foreach (string key in parsed.Keys) {
                _query[key] = parsed.GetValues(key);
            }
            _pathBase = "/";
            _absolutePath = url.AbsolutePath;
            _relativePath = _pathBase.GetAppRelativePath(_absolutePath, true);
            _headers["Host"] = new string[] { url.Host };
            _isSecure = url.Scheme == "https";
        }


        /* ====== Request factory methods ====== */

        public static Request Get(string url)
        {
            return new Request("GET", new Uri(url));
        }

        public static Request Post(string url)
        {
            return new Request("POST", new Uri(url));
        }

        public static Request Put(string url)
        {
            return new Request("PUT", new Uri(url));
        }

        public static Request Delete(string url)
        {
            return new Request("DELETE", new Uri(url));
        }

        public static Request Head(string url)
        {
            return new Request("HEAD", new Uri(url));
        }

        public static Request Options(string url)
        {
            return new Request("OPTIONS", new Uri(url));
        }


        /* ====== Modifiers ====== */


        private void AddValue(IDictionary<string, string[]> dict, string name, string value)
        {
            string[] values;
            if (!dict.TryGetValue(name, out values)) values = new string[0];
            dict[name] = values.Concat(new string[] { value }).ToArray();
        }

        public Request Form(string name, string value)
        {
            AddValue(_form, name, value);
            return this;
        }


        public Request Header(string name, string value)
        {
            AddValue(_headers, name, value);
            return this;
        }


        /* ====== IRequest implementation ====== */

        VirtualPath IRequest.AbsolutePath { get { return _absolutePath; } }

        IDictionary<string, string[]> IRequest.Form { get { return _form; } }

        RequestHeaders IRequest.Headers { get { return _headers; } }

        bool IRequest.IsSecure {  get { return _isSecure; } }

        string IRequest.Method { get { return _method; } }

        VirtualPath IRequest.Path { get { return _relativePath; } }

        VirtualPath IRequest.PathBase { get { return _pathBase; } }

        string IRequest.Protocol {  get { return _protocol; } }

        IDictionary<string, string[]> IRequest.Query { get { return _query; } }

        Uri IRequest.Url { get { return _url; } }
    }
}
