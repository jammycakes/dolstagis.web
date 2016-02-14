using System;
using System.Collections.Generic;
using System.Security.Cryptography;
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


        public Task Persist()
        {
            // Nothing to see here, move along please.
            return Task.FromResult(0);
        }

        public Task<object> GetItemAsync(string key)
        {
            object result;
            if (!Items.TryGetValue(key, out result)) result = null;
            return Task.FromResult<object>(result);
        }

        public Task SetItemAsync(string key, object value)
        {
            if (value == null && Items.ContainsKey(key)) {
                Items.Remove(key);
            }
            else {
                Items[key] = value;
            }
            return Task.FromResult(0);
        }
    }
}
