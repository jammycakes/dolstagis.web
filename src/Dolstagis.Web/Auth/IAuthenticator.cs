using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Http;
using Dolstagis.Web.Sessions;

namespace Dolstagis.Web.Auth
{
    public interface IAuthenticator
    {
        IUser GetUser(IHttpContext context);

        void SetUser(IHttpContext context, IUser user);
    }
}
