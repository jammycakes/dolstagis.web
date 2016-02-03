using System;
using System.Linq;

namespace Dolstagis.Web.Auth
{
    /// <summary>
    ///  Requires the user to be in all the specified roles.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class RequireAllAttribute : Attribute, IRequirement
    {
        private string[] _requiredRoles;

        public RequireAllAttribute(params string[] requiredRoles)
        {
            _requiredRoles = requiredRoles;
        }

        public bool IsDenied(IRequestContext context)
        {
            return context.User == null ||
                _requiredRoles.Any(x => !context.User.IsInRole(x));
        }
    }
}
