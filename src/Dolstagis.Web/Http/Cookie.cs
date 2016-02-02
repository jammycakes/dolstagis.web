using System;
using System.Text;

namespace Dolstagis.Web.Http
{
    /// <summary>
    ///  An HTTP cookie.
    /// </summary>
    public class Cookie
    {
        /// <summary>
        ///  Gets the name of the cookie.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        ///  Gets the cookie value.
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        ///  Gets or sets the cookie's path.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        ///  Gets or sets the cookie's domain name.
        /// </summary>
        public string Domain { get; set; }

        /// <summary>
        ///  Gets or sets a value indicating whether this is an HTTP-only cookie
        ///  (invisible to JavaScript).
        /// </summary>
        public bool HttpOnly { get; set; }

        /// <summary>
        ///  Gets or sets a value indicating whether this is a secure cookie.
        /// </summary>
        public bool Secure { get; set; }

        /// <summary>
        ///  Gets or sets the date and time that the cookie expires.
        ///  If not set, this will be a session cookie.
        /// </summary>
        public DateTime? Expires { get; set; }

        /// <summary>
        ///  Creates a new cookie.
        /// </summary>
        /// <param name="name">
        ///  The cookie name.
        /// </param>
        /// <param name="value">
        ///  The cookie value.
        /// </param>
        public Cookie(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string ToHeaderString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("{0}={1}", Name, HttpUtility.UrlEncode(Value, Encoding.UTF8));
            if (!String.IsNullOrWhiteSpace(Domain)) sb.Append("; Domain=" + Domain);
            sb.Append("; Path=" + Path);
            if (Expires.HasValue) sb.Append("; Expires=" + Expires.Value.ToString("R"));
            if (Secure) sb.Append("; Secure");
            if (HttpOnly) sb.Append("; HttpOnly");
            return sb.ToString();
        }
    }
}
