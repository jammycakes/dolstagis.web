using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

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
        public Task<ISession> GetSession(string sessionID)
        {
            InMemorySession session;
            if (sessionID != null && _sessions.TryGetValue(sessionID, out session))
            {
                return Task.FromResult<ISession>(session);
            }
            else
            {
                session = new InMemorySession(this);
                _sessions.TryAdd(session.ID, session);
            }
            return Task.FromResult<ISession>(session);
        }

        Task ISessionStore.Purge()
        {
            return Task.Run(() => Purge());
        }

        internal void EndSession(InMemorySession session)
        {
            _sessions.TryRemove(session.ID, out session);
        }
    }
}
