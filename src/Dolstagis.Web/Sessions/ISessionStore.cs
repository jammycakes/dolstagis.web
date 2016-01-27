namespace Dolstagis.Web.Sessions
{
    /// <summary>
    ///  Encapsulates a session store.
    /// </summary>
    public interface ISessionStore
    {
        /// <summary>
        ///  Gets the session object for a given session ID. If none exists,
        ///  creates it.
        /// </summary>
        /// <param name="sessionID">
        ///  The session ID, or null if no session has yet been started.
        /// </param>
        /// <returns>
        ///  The session, if present; if not, it will return a new <see cref="ISession"/>
        ///  instance. Note that in the latter case, the session ID will not be the same
        ///  as the one passed into this method.
        /// </returns>
        ISession GetSession(string sessionID);

        /// <summary>
        ///  Purges expired sessions.
        /// </summary>
        void Purge();
    }
}
