using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.Auth
{
    /// <summary>
    ///  Requires the user to be in none of the specified roles.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class DenyAttribute : Attribute, IRequirement
    {
        private string[] _requiredRoles;

        public DenyAttribute(params string[] requiredRoles)
        {
            _requiredRoles = requiredRoles;
        }

        public bool IsDenied(IHttpContext context)
        {
            return context.User != null &&
                _requiredRoles.Any(x => context.User.IsInRole(x));
        }
    }
}
