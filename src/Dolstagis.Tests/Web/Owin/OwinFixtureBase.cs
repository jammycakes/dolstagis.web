﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dolstagis.Tests.Web.Owin
{
    public class OwinFixtureBase
    {
        /// <summary>
        ///  Creates a sample Owin context containing all the keys that are
        ///  required per the Owin 1.0 specification, section 3.2.1.
        /// </summary>
        /// <remarks>
        ///  This context MUST NOT be altered to include any keys not listed in
        ///  section 3.2.1 of the Owin specification. In particular, it must not
        ///  include the common keys listed in section 6 of the Owin "Common
        ///  Keys" document, as these are implementation-dependent and are not
        ///  guaranteed to be present.
        /// </remarks>
        /// <returns></returns>

        public IDictionary<string, object> BuildDefaultOwinEnvironment()
        {
            return new Dictionary<string, object>() {
                { "owin.RequestBody", Stream.Null },
                { "owin.RequestHeaders",
                    new Dictionary<string, string[]> {
                        { "Host", new string[] { "localhost" } }
                    }
                },
                { "owin.RequestMethod", "GET" },
                { "owin.RequestPath", "/" },
                { "owin.RequestPathBase", String.Empty },
                { "owin.RequestProtocol", "HTTP/1.1" },
                { "owin.RequestQueryString", String.Empty },
                { "owin.RequestScheme", "http" },

                { "owin.ResponseBody", Stream.Null },
                { "owin.ResponseHeaders", new Dictionary<string, string[]>() },
                { "owin.ResponseStatusCode", 200 },
                { "owin.ResponseReasonPhrase", "OK" },
                { "owin.ResponseProtocol", "HTTP/1.1" },

                { "owin.CallCancelled", new CancellationToken() },
                { "owin.Version", "1.0" }
            };
        }
    }
}