using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web;

namespace Dolstagis.Tests.Web.TestFeatures.Handlers
{
    [Route("/")]
    public class RootHandler : Handler
    {
        /// <summary>
        ///  A synchronous action returning a status object 200/OK.
        /// </summary>
        /// <returns></returns>

        public object Get()
        {
            return "Hello GET";
        }

        /// <summary>
        ///  An asynchronous action returning an object.
        /// </summary>
        /// <returns></returns>

        public async Task<object> Post()
        {
            return await Task<object>.Run(() => "Hello POST");
        }

        /// <summary>
        ///  An asynchronous action returning a string.
        /// </summary>
        /// <returns></returns>

        public async Task<string> Put()
        {
            return await Task<string>.Run(() => "Hello PUT");
        }

        /// <summary>
        ///  An asynchronous action returning a Task`1[string].
        /// </summary>
        /// <returns></returns>

        public Task<string> Delete()
        {
            return Task<string>.Run(() => "Hello DELETE");
        }
    }
}
