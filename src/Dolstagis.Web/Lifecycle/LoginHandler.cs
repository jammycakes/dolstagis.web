using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.Lifecycle
{
    public class LoginHandler : ILoginHandler
    {
        public string LoginUrl { get; set; }

        public LoginHandler()
        {
            LoginUrl = "~/login";
        }

        public object GetLogin(IRequestContext context)
        {
            return new RedirectResult(LoginUrl, Status.SeeOther);
        }
    }
}
