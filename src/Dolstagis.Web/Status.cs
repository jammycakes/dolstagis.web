﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Errors;

namespace Dolstagis.Web
{
    /// <summary>
    ///  Represents an HTTP status code.
    /// </summary>

    [Serializable]
    public sealed class Status
    {
        private static IDictionary<int, Status> _statuses = new Dictionary<int, Status>();

        public int Code { get; private set; }

        public string Description { get; private set; }

        public string Message { get; set; }

        private Status(int code, string description, string message)
        {
            this.Code = code;
            this.Description = description;
            this.Message = message;
            _statuses.Add(this.Code, this);
        }

        private Status(int code, string description)
            : this(code, description, description)
        {
        }

        /// <summary>
        ///  Throws an exception with this status code.
        /// </summary>

        public void Throw()
        {
            throw new HttpStatusException(this);
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
        public static readonly Status MovedPermanently = new Status(301, "Moved Permanently");
        public static readonly Status Found = new Status(302, "Found");
        public static readonly Status SeeOther = new Status(303, "See Other");
        public static readonly Status NotModified = new Status(304, "Not Modified");
        public static readonly Status UseProxy = new Status(305, "Use Proxy");
        public static readonly Status SwitchProxy = new Status(306, "Switch Proxy");
        public static readonly Status TemporaryRedirect = new Status(307, "Temporary Redirect");
        public static readonly Status ResumeIncomplete = new Status(308, "Resume Incomplete");

        public static readonly Status BadRequest = new Status
            (400, "Bad Request", StatusMessages.Message400BadRequest);
        public static readonly Status Unauthorized = new Status
            (401, "Unauthorized", StatusMessages.Message401Unauthorized);
        public static readonly Status PaymentRequired = new Status
            (402, "Payment Required");
        public static readonly Status Forbidden = new Status
            (403, "Forbidden", StatusMessages.Message403Forbidden);
        public static readonly Status NotFound = new Status
            (404, "Not Found", StatusMessages.Message404NotFound);
        public static readonly Status MethodNotAllowed = new Status
            (405, "Method Not Allowed", StatusMessages.Message405MethodNotAllowed);
        public static readonly Status NotAcceptable = new Status
            (406, "Not Acceptable", StatusMessages.Message406NotAcceptable);
        public static readonly Status ProxyAuthenticationRequired = new Status
            (407, "Proxy Authentication Required", StatusMessages.Message407ProxyAuthenticationRequired);
        public static readonly Status RequestTimeout = new Status
            (408, "Request Timeout", StatusMessages.Message408RequestTimeout);
        public static readonly Status Conflict = new Status(409, "Conflict");
        public static readonly Status Gone = new Status
            (410, "Gone", StatusMessages.Message410Gone);
        public static readonly Status LengthRequired = new Status
            (411, "Length Required", StatusMessages.Message411LengthRequired);
        public static readonly Status PreconditionFailed = new Status
            (412, "Precondition Failed", StatusMessages.Message412PreconditionFailed);
        public static readonly Status RequestEntityTooLarge = new Status
            (413, "Request Entity Too Large", StatusMessages.Message413RequestEntityTooLarge);
        public static readonly Status RequestUriTooLong = new Status
            (414, "Request Uri Too Long", StatusMessages.Message414RequestUriTooLong);
        public static readonly Status UnsupportedMediaType = new Status
            (415, "Unsupported Media Type", StatusMessages.Message415UnsupportedMediaType);
        public static readonly Status RequestedRangeNotSatisfiable = new Status
            (416, "Requested Range Not Satisfiable", StatusMessages.Message416RequestedRangeNotSatisfiable);
        public static readonly Status ExpectationFailed = new Status
            (417, "Expectation Failed", StatusMessages.Message417ExpectationFailed);
        public static readonly Status ImATeapot = new Status
            (418, "Im ATeapot", StatusMessages.Message418ImATeapot);
        public static readonly Status EnhanceYourCalm = new Status
            (420, "Enhance Your Calm", StatusMessages.Message420EnhanceYourCalm);
        public static readonly Status UnprocessableEntity = new Status
            (422, "Unprocessable Entity", StatusMessages.Message422UnprocessableEntity);
        public static readonly Status Locked = new Status
            (423, "Locked", StatusMessages.Message423Locked);
        public static readonly Status FailedDependency = new Status
            (424, "Failed Dependency", StatusMessages.Message424FailedDependency);
        public static readonly Status UnorderedCollection = new Status
            (425, "Unordered Collection");
        public static readonly Status UpgradeRequired = new Status
            (426, "Upgrade Required", StatusMessages.Message426UpgradeRequired);
        public static readonly Status TooManyRequests = new Status
            (429, "Too Many Requests", StatusMessages.Message429TooManyRequests);
        public static readonly Status NoResponse = new Status(444, "No Response");
        public static readonly Status RetryWith = new Status(449, "Retry With");
        public static readonly Status BlockedByWindowsParentalControls = new Status(450, "Blocked By Windows Parental Controls");
        public static readonly Status ClientClosedRequest = new Status(499, "Client Closed Request");

        public static readonly Status InternalServerError = new Status
            (500, "Internal Server Error", StatusMessages.Message500InternalServerError);
        public static readonly Status NotImplemented = new Status
            (501, "Not Implemented", StatusMessages.Message501NotImplemented);
        public static readonly Status BadGateway = new Status
            (502, "Bad Gateway", StatusMessages.Message502BadGateway);
        public static readonly Status ServiceUnavailable = new Status
            (503, "Service Unavailable", StatusMessages.Message503ServiceUnavailable);
        public static readonly Status GatewayTimeout = new Status
            (504, "Gateway Timeout", StatusMessages.Message504GatewayTimeout);
        public static readonly Status HttpVersionNotSupported = new Status(505, "Http Version Not Supported");
        public static readonly Status VariantAlsoNegotiates = new Status
            (506, "Variant Also Negotiates", StatusMessages.Message506VariantAlsoNegotiates);
        public static readonly Status InsufficientStorage = new Status
            (507, "Insufficient Storage", StatusMessages.Message507InsufficientStorage);
        public static readonly Status BandwidthLimitExceeded = new Status(509, "Bandwidth Limit Exceeded");
        public static readonly Status NotExtended = new Status
            (510, "Not Extended", StatusMessages.Message510NotExtended);
    }
}
