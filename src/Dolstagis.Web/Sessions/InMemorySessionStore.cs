using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Runtime.CompilerServices;

namespace Dolstagis.Web.Sessions
{
    public class InMemorySessionStore : ISessionStore
    {
        private static ConcurrentDictionary<string, InMemorySession> _sessions
            = new ConcurrentDictionary<string, InMemorySession>();

        private static void Purge()
        {
            foreach (string key in _sessions.Keys)
            {
                InMemorySession session;
                if (_sessions.TryGetValue(key, out session) && session.Expires < DateTime.UtcNow)
                {
                    _sessions.TryRemove(key, out session);
                }
            }
        }

        private static readonly Timer timer = new Timer(x => Purge(), null, 60000, 60000);


        [MethodImpl(MethodImplOptions.Synchronized)]
        public ISession GetSession(string sessionID)
        {
            InMemorySession session;
            if (sessionID != null && _sessions.TryGetValue(sessionID, out session))
            {
                return session;
            }
            else
            {
                session = new InMemorySession(this);
                _sessions.TryAdd(session.ID, session);
            }
            return session;
        }

        void ISessionStore.Purge()
        {
            Purge();
        }

        internal void EndSession(InMemorySession session)
        {
            _sessions.TryRemove(session.ID, out session);
        }
    }
}
