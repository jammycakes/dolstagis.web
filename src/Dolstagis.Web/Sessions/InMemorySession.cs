using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.Sessions
{
    public class InMemorySession : ISession
    {
        private static RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

        private InMemorySessionStore _store;
        private IDictionary<string, object> _items
            = new Dictionary<string, object>();

        public string ID { get; private set; }

        public TimeSpan? Lifetime { get; set; }

        public DateTime? Expires { get; private set; }

        public InMemorySession(InMemorySessionStore store)
        {
            _store = store;
            var bytes = new byte[32];
            rng.GetBytes(bytes);
            ID = Convert.ToBase64String(bytes);
            Lifetime = TimeSpan.FromMinutes(20);
        }

        public IDictionary<string, object> Items
        {
            get
            {
                Expires = Lifetime.HasValue ? DateTime.UtcNow + Lifetime : null;
                return _items;
            }
        }

        public void End()
        {
            _store.EndSession(this);
        }


        public async Task Persist()
        {
            // Nothing to see here, move along please.
            await Task.Yield();
        }
    }
}
