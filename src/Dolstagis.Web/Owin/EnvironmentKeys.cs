namespace Dolstagis.Web.Owin
{
    public static class EnvironmentKeys
    {
        /* ====== Owin spec section 3.2.1 ====== */

        public const string RequestBody = "owin.RequestBody";
        public const string RequestHeaders = "owin.RequestHeaders";
        public const string RequestMethod = "owin.RequestMethod";
        public const string RequestPath = "owin.RequestPath";
        public const string RequestPathBase = "owin.RequestPathBase";
        public const string RequestProtocol = "owin.RequestProtocol";
        public const string RequestQueryString = "owin.RequestQueryString";
        public const string RequestScheme = "owin.RequestScheme";


        /* ====== Owin spec section 3.2.2 ====== */

        public const string ResponseBody = "owin.ResponseBody";
        public const string ResponseHeaders = "owin.ResponseHeaders";
        public const string ResponseStatusCode = "owin.ResponseStatusCode";
        public const string ResponseReasonPhrase = "owin.ResponseReasonPhrase";
        public const string ResponseProtocol = "owin.ResponseProtocol";


        /* ====== Owin spec section 3.2.3 ====== */

        public const string CallCancelled = "owin.CallCancelled";
        public const string OwinVersion = "owin.Version";


        /* ====== Extensions defined in Owin Common Keys ====== */

        public static class Ssl
        {
            public const string ClientCertificate = "ssl.ClientCertificate";
        }

        public static class Server
        {
            public const string RemoteIpAddress = "server.RemoteIpAddress";
            public const string RemotePort = "server.RemotePort";
            public const string LocalIpAddress = "server.LocalIpAddress";
            public const string LocalPort = "server.LocalPort";
            public const string IsLocal = "server.IsLocal";
            public const string Capabilities = "server.Capabilities";
            public const string OnSendingHeaders = "server.OnSendingHeaders";
        }

        public static class Host
        {
            public const string TraceOutput = "host.TraceOutput";
            public const string Addresses = "host.Addresses";
        }
    }
}