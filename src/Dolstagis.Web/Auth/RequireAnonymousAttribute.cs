using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.Auth
{
    /// <summary>
    ///  Requires the user to be logged out.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class RequireAnonymousAttribute: Attribute, IRequirement
    {
        public bool IsDenied(IRequestContext context)
        {
            return context.User != null;
        }
    }
}
