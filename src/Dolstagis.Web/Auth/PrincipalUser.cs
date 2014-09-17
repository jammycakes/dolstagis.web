using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.Auth
{
    public class PrincipalUser : IUser
    {
        private IPrincipal _principal;

        public PrincipalUser(IPrincipal principal)
        {
            _principal = principal;
        }

        public string UserName
        {
            get { return _principal.Identity.Name; }
        }

        public bool IsInRole(string role)
        {
            return _principal.IsInRole(role);
        }
    }
}
