using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace Dolstagis.Web.Sessions
{
    public class InMemorySessionStore : ISessionStore
    {
        private static ConcurrentDictionary<string, InMemorySession> _sessions
            = new ConcurrentDictionary<string, InMemorySession>();

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

        public void Purge(DateTime now)
        {
            foreach (string key in _sessions.Keys)
            {
                InMemorySession session;
                if (_sessions.TryGetValue(key, out session) && session.Expires < now)
                {
                    _sessions.TryRemove(key, out session);
                }
            }
        }

        internal void EndSession(InMemorySession session)
        {
            _sessions.TryRemove(session.ID, out session);
        }


        public void Purge()
        {
            Purge(DateTime.UtcNow);
        }
    }
}
