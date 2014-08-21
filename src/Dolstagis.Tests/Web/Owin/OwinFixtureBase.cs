using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dolstagis.Web.Owin;

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

        protected IDictionary<string, object> BuildDefaultOwinEnvironment()
        {
            return new Dictionary<string, object>() {
                { EnvironmentKeys.RequestBody, Stream.Null },
                { EnvironmentKeys.RequestBody,
                    new Dictionary<string, string[]> {
                        { "Host", new string[] { "localhost" } }
                    }
                },
                { EnvironmentKeys.RequestMethod, "GET" },
                { EnvironmentKeys.RequestPath, "/" },
                { EnvironmentKeys.RequestPathBase, String.Empty },
                { EnvironmentKeys.RequestProtocol, "HTTP/1.1" },
                { EnvironmentKeys.RequestQueryString, String.Empty },
                { EnvironmentKeys.RequestScheme, "http" },

                { EnvironmentKeys.ResponseBody, Stream.Null },
                { EnvironmentKeys.ResponseHeaders, new Dictionary<string, string[]>() },
                { EnvironmentKeys.ResponseStatusCode, 200 },
                { EnvironmentKeys.ResponseReasonPhrase, "OK" },
                { EnvironmentKeys.ResponseProtocol, "HTTP/1.1" },

                { EnvironmentKeys.CallCancelled, new CancellationToken() },
                { EnvironmentKeys.OwinVersion, "1.0" }
            };
        }
    }
}
