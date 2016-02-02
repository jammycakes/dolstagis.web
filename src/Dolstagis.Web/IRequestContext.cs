using System;
using System.Threading.Tasks;
using Dolstagis.Web.Auth;
using Dolstagis.Web.Http;
using Dolstagis.Web.Sessions;

namespace Dolstagis.Web
{
    /// <summary>
    ///  Encapsulates the request, response and related information for the
    ///  current HTTP request.
    /// </summary>
    public interface IRequestContext
    {
        /// <summary>
        ///  Gets an interface to the IOC container for this request.
        /// </summary>
        IServiceProvider Container { get; }

        /// <summary>
        ///  Gets the <see cref="IRequest"/> instance containing the data from
        ///  the HTTP request.
        /// </summary>
        IRequest Request { get; }

        /// <summary>
        ///  Gets the <see cref="IResponse"/> instance containing the data to be
        ///  sent back to the client in the HTTP response.
        /// </summary>
        IResponse Response { get; }

        /// <summary>
        ///  Gets the <see cref="ISession"/> instance containing session data.
        /// </summary>
        ISession Session { get; }

        /// <summary>
        ///  Gets the <see cref="IUser"/> instance representing the current
        ///  logged in user. If no user is logged in, returns null.
        /// </summary>
        IUser User { get; }

        /// <summary>
        ///  Gets the <see cref="ISession"/> instance containing session data
        ///  asynchronously.
        /// </summary>
        /// <returns>The session object.</returns>
        Task<ISession> GetSessionAsync();

        /// <summary>
        ///  Gets the <see cref="IUser"/> instance representing the current
        ///  logged in user asynchronously. If no user is logged in, returns
        ///  null.
        /// </summary>
        /// <returns>The user object, or null for anonymous requests.</returns>
        Task<IUser> GetUserAsync();
    }
}