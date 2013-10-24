using System;
namespace Dolstagis.Web.Http
{
    public interface ISessionCookieBuilder
    {
        Cookie CreateSessionCookie(string sessionID);
    }
}
