using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web
{
    /// <summary>
    ///  Represents an HTTP status code.
    /// </summary>

    public sealed class Status
    {
        private static IDictionary<int, Status> _statuses = new Dictionary<int, Status>();

        public int Code { get; private set; }

        public string Description { get; private set; }

        private Status(int code, string description)
        {
            this.Code = code;
            this.Description = description;
            _statuses.Add(this.Code, this);
        }

        /// <summary>
        ///  Gets an HTTP status object by code.
        /// </summary>
        /// <param name="code">
        ///  The HTTP status code.
        /// </param>
        /// <returns>
        ///  A <see cref="Status"/> instance, or null if undefined.
        /// </returns>

        public Status ByCode(int code)
        {
            Status result;
            return _statuses.TryGetValue(code, out result) ? result : null;
        }

        // The values are based on the list found at http://en.wikipedia.org/wiki/List_of_HTTP_status_codes

        public static readonly Status Continue = new Status(100, "Continue");
        public static readonly Status SwitchingProtocols = new Status(101, "Switching Protocols");
        public static readonly Status Processing = new Status(102, "Processing");
        public static readonly Status Checkpoint = new Status(103, "Checkpoint");
        public static readonly Status OK = new Status(200, "OK");
        public static readonly Status Created = new Status(201, "Created");
        public static readonly Status Accepted = new Status(202, "Accepted");
        public static readonly Status NonAuthoritativeInformation = new Status(203, "Non Authoritative Information");
        public static readonly Status NoContent = new Status(204, "No Content");
        public static readonly Status ResetContent = new Status(205, "Reset Content");
        public static readonly Status PartialContent = new Status(206, "Partial Content");
        public static readonly Status MultipleStatus = new Status(207, "Multiple Status");
        public static readonly Status IMUsed = new Status(226, "IM Used");
        public static readonly Status MultipleChoices = new Status(300, "Multiple Choices");
        public static readonly Status MovedPermanently = new Status(301, "Moved Permanently");
        public static readonly Status Found = new Status(302, "Found");
        public static readonly Status SeeOther = new Status(303, "See Other");
        public static readonly Status NotModified = new Status(304, "Not Modified");
        public static readonly Status UseProxy = new Status(305, "Use Proxy");
        public static readonly Status SwitchProxy = new Status(306, "Switch Proxy");
        public static readonly Status TemporaryRedirect = new Status(307, "Temporary Redirect");
        public static readonly Status ResumeIncomplete = new Status(308, "Resume Incomplete");
        public static readonly Status BadRequest = new Status(400, "Bad Request");
        public static readonly Status Unauthorized = new Status(401, "Unauthorized");
        public static readonly Status PaymentRequired = new Status(402, "Payment Required");
        public static readonly Status Forbidden = new Status(403, "Forbidden");
        public static readonly Status NotFound = new Status(404, "Not Found");
        public static readonly Status MethodNotAllowed = new Status(405, "Method Not Allowed");
        public static readonly Status NotAcceptable = new Status(406, "Not Acceptable");
        public static readonly Status ProxyAuthenticationRequired = new Status(407, "Proxy Authentication Required");
        public static readonly Status RequestTimeout = new Status(408, "Request Timeout");
        public static readonly Status Conflict = new Status(409, "Conflict");
        public static readonly Status Gone = new Status(410, "Gone");
        public static readonly Status LengthRequired = new Status(411, "Length Required");
        public static readonly Status PreconditionFailed = new Status(412, "Precondition Failed");
        public static readonly Status RequestEntityTooLarge = new Status(413, "Request Entity Too Large");
        public static readonly Status RequestUriTooLong = new Status(414, "Request Uri Too Long");
        public static readonly Status UnsupportedMediaType = new Status(415, "Unsupported Media Type");
        public static readonly Status RequestedRangeNotSatisfiable = new Status(416, "Requested Range Not Satisfiable");
        public static readonly Status ExpectationFailed = new Status(417, "Expectation Failed");
        public static readonly Status ImATeapot = new Status(418, "Im ATeapot");
        public static readonly Status EnhanceYourCalm = new Status(420, "Enhance Your Calm");
        public static readonly Status UnprocessableEntity = new Status(422, "Unprocessable Entity");
        public static readonly Status Locked = new Status(423, "Locked");
        public static readonly Status FailedDependency = new Status(424, "Failed Dependency");
        public static readonly Status UnorderedCollection = new Status(425, "Unordered Collection");
        public static readonly Status UpgradeRequired = new Status(426, "Upgrade Required");
        public static readonly Status TooManyRequests = new Status(429, "Too Many Requests");
        public static readonly Status NoResponse = new Status(444, "No Response");
        public static readonly Status RetryWith = new Status(449, "Retry With");
        public static readonly Status BlockedByWindowsParentalControls = new Status(450, "Blocked By Windows Parental Controls");
        public static readonly Status ClientClosedRequest = new Status(499, "Client Closed Request");
        public static readonly Status InternalServerError = new Status(500, "Internal Server Error");
        public static readonly Status NotImplemented = new Status(501, "Not Implemented");
        public static readonly Status BadGateway = new Status(502, "Bad Gateway");
        public static readonly Status ServiceUnavailable = new Status(503, "Service Unavailable");
        public static readonly Status GatewayTimeout = new Status(504, "Gateway Timeout");
        public static readonly Status HttpVersionNotSupported = new Status(505, "Http Version Not Supported");
        public static readonly Status VariantAlsoNegotiates = new Status(506, "Variant Also Negotiates");
        public static readonly Status InsufficientStorage = new Status(507, "Insufficient Storage");
        public static readonly Status BandwidthLimitExceeded = new Status(509, "Bandwidth Limit Exceeded");
        public static readonly Status NotExtended = new Status(510, "Not Extended");
    }
}
