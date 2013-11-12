using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.Lifecycle
{
    public class LoginHandler : ILoginHandler
    {
        public object GetLogin(IHttpContext context)
        {
            return new RedirectResult("/login", Status.SeeOther);
        }
    }
}
