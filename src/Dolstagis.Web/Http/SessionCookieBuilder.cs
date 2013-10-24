using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.Http
{
    public class SessionCookieBuilder : Dolstagis.Web.Http.ISessionCookieBuilder
    {
        public TimeSpan Lifetime { get; set; }

        public SessionCookieBuilder()
        {
            Lifetime = TimeSpan.FromDays(365);
        }

        public Cookie CreateSessionCookie(string sessionID)
        {
            var cookie = new Cookie(Constants.SessionKey, sessionID);
            if (Lifetime > TimeSpan.Zero)
            {
                cookie.Expires = DateTime.UtcNow.Add(Lifetime);
            }
            cookie.HttpOnly = true;
            return cookie;
        }
    }
}
