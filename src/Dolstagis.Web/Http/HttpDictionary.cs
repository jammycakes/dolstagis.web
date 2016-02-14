using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Dolstagis.Web.Http
{
    public class HttpDictionary : IDictionary<string, string[]>
    {
        private IDictionary<string, string[]> inner;

        public HttpDictionary(IDictionary<string, string[]> inner)
        {
            this.inner = inner;
        }

        #region /* ====== IDictionary<string, string[]> implementation ====== */

        public void Add(string key, string[] value)
        {
            inner.Add(key, value);
        }

        public bool ContainsKey(string key)
        {
            return inner.ContainsKey(key);
        }

        public ICollection<string> Keys
        {
            get { return inner.Keys; }
        }

        public bool Remove(string key)
        {
            return inner.Remove(key);
        }

        public bool TryGetValue(string key, out string[] value)
        {
            return inner.TryGetValue(key, out value);
        }

        public ICollection<string[]> Values
        {
            get { return inner.Values; }
        }

        public string[] this[string key]
        {
            get { return inner[key]; }
            set { inner[key] = value; }
        }

        public void Add(KeyValuePair<string, string[]> item)
        {
            inner.Add(item);
        }

        public void Clear()
        {
            inner.Clear();
        }

        public bool Contains(KeyValuePair<string, string[]> item)
        {
            return inner.Contains(item);
        }

        public void CopyTo(KeyValuePair<string, string[]>[] array, int arrayIndex)
        {
            inner.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return inner.Count; }
        }

        public bool IsReadOnly
        {
            get { return inner.IsReadOnly; }
        }

        public bool Remove(KeyValuePair<string, string[]> item)
        {
            return inner.Remove(item);
        }

        public IEnumerator<KeyValuePair<string, string[]>> GetEnumerator()
        {
            return inner.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)inner).GetEnumerator();
        }

        #endregion

        /* ====== Additional methods ====== */

        /// <summary>
        ///  Gets the first instance of a header, or null of none.
        /// </summary>
        /// <param name="key">
        ///  The key to query.
        /// </param>
        /// <returns>
        ///  The first value represented by the key, or null if not found.
        /// </returns>

        protected string GetValue(string key)
        {
            string[] results;
            return this.TryGetValue(key, out results) ? results.FirstOrDefault() : null;
        }

        protected void SetValue(string key, string value)
        {
            this[key] = new string[] { value };
        }


        /* ====== Content-Type ====== */

        public string ContentType
        {
            get { return this.GetValue("Content-Type"); }
            set { this.SetValue("Content-Type", value); }
        }

        private const string disallowedMimeCharacters = @"()<>@,;:\""[]?.=";

        private static readonly Regex reInvalidMimeCharacters =
            new Regex(@"[\s" + Regex.Escape(disallowedMimeCharacters) + "]");

        public string MimeType
        {
            get {
                string contentType = this.ContentType;
                if (contentType == null) return null;
                return contentType.Split(';').First().Trim();
            }
            set {
                if (value == null) throw new ArgumentNullException();
                string mt = value.Trim();
                if (reInvalidMimeCharacters.Match(mt).Success) {
                    throw new ArgumentException(
                        "MIME type can not contain whitespace or any of the following " +
                        "characters: " + disallowedMimeCharacters);
                }
                string contentType = mt;
                Encoding enc = Encoding;
                if (enc != null) contentType += ";charset=" + enc.WebName;
                this.ContentType = value;
            }
        }


        private static readonly Regex reCharset = new Regex(@";\s*charset\s*=\s*(.*?)\s*(?:;|$)");

        public Encoding Encoding
        {
            get {
                string contentType = this.ContentType;
                if (contentType == null) return null;
                var match = reCharset.Match(contentType);
                if (!match.Success) return null;
                return Encodings.Lookup(match.Groups[1].Value);
            }
            set {
                string contentType = MimeType ?? "text/plain";
                if (value != null) contentType += ";charset=" + value.WebName;
                this.ContentType = contentType;
            }
        }
    }
}
