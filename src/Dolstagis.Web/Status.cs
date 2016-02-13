using System;
using System.Collections.Generic;
using Dolstagis.Web.Errors;

namespace Dolstagis.Web
{
    /// <summary>
    ///  Represents an HTTP status code.
    /// </summary>

    [Serializable]
    public class Status
    {
        private static IDictionary<int, Status> _statuses = new Dictionary<int, Status>();

        public int Code { get; private set; }

        public string Description { get; private set; }

        public string Message { get; set; }

        internal Status(int code, string description, string message)
        {
            this.Code = code;
            this.Description = description;
            this.Message = message;
            _statuses.Add(this.Code, this);
        }

        internal Status(int code, string description)
            : this(code, description, description)
        {
        }

        /// <summary>
        ///  Gets a string representation of this HTTP status.
        /// </summary>
        /// <returns>
        ///  The string representation of this status.
        /// </returns>

        public override string ToString()
        {
            return String.Format("{0} {1}", this.Code, this.Description);
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

        public static Status ByCode(int code)
        {
            Status result;
            return _statuses.TryGetValue(code, out result) ? result : null;
        }

        // The values are based on the list found at http://en.wikipedia.org/wiki/List_of_HTTP_status_codes
        // Long descriptions loosely based on those in the Apache web server

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
        public static readonly Status MovedPermanently = new Status
            (301, "Moved Permanently", StatusMessages.Message301MovedPermanently);
        public static readonly Status Found = new Status
            (302, "Found", StatusMessages.Message302Found);
        public static readonly Status SeeOther = new Status
            (303, "See Other", StatusMessages.Message303SeeOther);
        public static readonly Status NotModified = new Status(304, "Not Modified");
        public static readonly Status UseProxy = new Status
            (305, "Use Proxy", StatusMessages.Message305UseProxy);
        [Obsolete("This status has been removed from the HTTP/1.1 specification. See RFC2616 §10.3.7")]
        public static readonly Status SwitchProxy = new Status(306, "Switch Proxy");
        public static readonly Status TemporaryRedirect = new Status
            (307, "Temporary Redirect", StatusMessages.Message307TemporaryRedirect);
        public static readonly Status ResumeIncomplete = new Status(308, "Resume Incomplete");

        public static readonly ErrorStatus BadRequest = new ErrorStatus
            (400, "Bad Request", StatusMessages.Message400BadRequest);
        public static readonly ErrorStatus Unauthorized = new ErrorStatus
            (401, "Unauthorized", StatusMessages.Message401Unauthorized);
        public static readonly ErrorStatus PaymentRequired = new ErrorStatus
            (402, "Payment Required");
        public static readonly ErrorStatus Forbidden = new ErrorStatus
            (403, "Forbidden", StatusMessages.Message403Forbidden);
        public static readonly ErrorStatus NotFound = new ErrorStatus
            (404, "Not Found", StatusMessages.Message404NotFound);
        public static readonly ErrorStatus MethodNotAllowed = new ErrorStatus
            (405, "Method Not Allowed", StatusMessages.Message405MethodNotAllowed);
        public static readonly ErrorStatus NotAcceptable = new ErrorStatus
            (406, "Not Acceptable", StatusMessages.Message406NotAcceptable);
        public static readonly ErrorStatus ProxyAuthenticationRequired = new ErrorStatus
            (407, "Proxy Authentication Required", StatusMessages.Message407ProxyAuthenticationRequired);
        public static readonly ErrorStatus RequestTimeout = new ErrorStatus
            (408, "Request Timeout", StatusMessages.Message408RequestTimeout);
        public static readonly ErrorStatus Conflict = new ErrorStatus(409, "Conflict");
        public static readonly ErrorStatus Gone = new ErrorStatus
            (410, "Gone", StatusMessages.Message410Gone);
        public static readonly ErrorStatus LengthRequired = new ErrorStatus
            (411, "Length Required", StatusMessages.Message411LengthRequired);
        public static readonly ErrorStatus PreconditionFailed = new ErrorStatus
            (412, "Precondition Failed", StatusMessages.Message412PreconditionFailed);
        public static readonly ErrorStatus RequestEntityTooLarge = new ErrorStatus
            (413, "Request Entity Too Large", StatusMessages.Message413RequestEntityTooLarge);
        public static readonly ErrorStatus RequestUriTooLong = new ErrorStatus
            (414, "Request Uri Too Long", StatusMessages.Message414RequestUriTooLong);
        public static readonly ErrorStatus UnsupportedMediaType = new ErrorStatus
            (415, "Unsupported Media Type", StatusMessages.Message415UnsupportedMediaType);
        public static readonly ErrorStatus RequestedRangeNotSatisfiable = new ErrorStatus
            (416, "Requested Range Not Satisfiable", StatusMessages.Message416RequestedRangeNotSatisfiable);
        public static readonly ErrorStatus ExpectationFailed = new ErrorStatus
            (417, "Expectation Failed", StatusMessages.Message417ExpectationFailed);
        public static readonly ErrorStatus ImATeapot = new ErrorStatus
            (418, "Im ATeapot", StatusMessages.Message418ImATeapot);
        public static readonly ErrorStatus EnhanceYourCalm = new ErrorStatus
            (420, "Enhance Your Calm", StatusMessages.Message420EnhanceYourCalm);
        public static readonly ErrorStatus UnprocessableEntity = new ErrorStatus
            (422, "Unprocessable Entity", StatusMessages.Message422UnprocessableEntity);
        public static readonly ErrorStatus Locked = new ErrorStatus
            (423, "Locked", StatusMessages.Message423Locked);
        public static readonly ErrorStatus FailedDependency = new ErrorStatus
            (424, "Failed Dependency", StatusMessages.Message424FailedDependency);
        public static readonly ErrorStatus UnorderedCollection = new ErrorStatus
            (425, "Unordered Collection");
        public static readonly ErrorStatus UpgradeRequired = new ErrorStatus
            (426, "Upgrade Required", StatusMessages.Message426UpgradeRequired);
        public static readonly ErrorStatus TooManyRequests = new ErrorStatus
            (429, "Too Many Requests", StatusMessages.Message429TooManyRequests);
        public static readonly ErrorStatus NoResponse = new ErrorStatus(444, "No Response");
        public static readonly ErrorStatus RetryWith = new ErrorStatus(449, "Retry With");
        public static readonly ErrorStatus BlockedByWindowsParentalControls
            = new ErrorStatus(450, "Blocked By Windows Parental Controls");
        public static readonly ErrorStatus ClientClosedRequest 
            = new ErrorStatus(499, "Client Closed Request");

        public static readonly ErrorStatus InternalServerError = new ErrorStatus
            (500, "Internal Server Error", StatusMessages.Message500InternalServerError);
        public static readonly ErrorStatus NotImplemented = new ErrorStatus
            (501, "Not Implemented", StatusMessages.Message501NotImplemented);
        public static readonly ErrorStatus BadGateway = new ErrorStatus
            (502, "Bad Gateway", StatusMessages.Message502BadGateway);
        public static readonly ErrorStatus ServiceUnavailable = new ErrorStatus
            (503, "Service Unavailable", StatusMessages.Message503ServiceUnavailable);
        public static readonly ErrorStatus GatewayTimeout = new ErrorStatus
            (504, "Gateway Timeout", StatusMessages.Message504GatewayTimeout);
        public static readonly ErrorStatus HttpVersionNotSupported = new ErrorStatus(505, "Http Version Not Supported");
        public static readonly ErrorStatus VariantAlsoNegotiates = new ErrorStatus
            (506, "Variant Also Negotiates", StatusMessages.Message506VariantAlsoNegotiates);
        public static readonly ErrorStatus InsufficientStorage = new ErrorStatus
            (507, "Insufficient Storage", StatusMessages.Message507InsufficientStorage);
        public static readonly ErrorStatus BandwidthLimitExceeded = new ErrorStatus(509, "Bandwidth Limit Exceeded");
        public static readonly ErrorStatus NotExtended = new ErrorStatus
            (510, "Not Extended", StatusMessages.Message510NotExtended);
    }
}
