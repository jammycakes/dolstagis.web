using System;
using Dolstagis.Web.Http;

namespace Dolstagis.Web.Sessions
{
    public interface ISessionCookieBuilder
    {
        Cookie CreateSessionCookie(string sessionID);
    }
}
