using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dolstagis.Web.Sessions
{
    public interface ISession
    {
        /// <summary>
        ///  Gets the session ID.
        /// </summary>
        string ID { get; }

        /// <summary>
        ///  Gets the items stored in the session.
        /// </summary>
        IDictionary<string, object> Items { get; }

        /// <summary>
        ///  Gets the requested item asynchronously.
        /// </summary>
        /// <param name="key">The key of the item to fetch.</param>
        /// <returns>The item to fetch, or null if not found.</returns>
        Task<object> GetItemAsync(string key);

        /// <summary>
        ///  Sets the requested item asynchronously.
        /// </summary>
        /// <param name="key">The key of the item to set.</param>
        /// <param name="value">The item to set, or null to clear it.</param>
        /// <returns>An awaitable.</returns>
        Task SetItemAsync(string key, object value);

        /// <summary>
        ///  Gets the date and time that the session expires.
        ///  If it never expires, returns null.
        /// </summary>
        DateTime? Expires { get; }

        /// <summary>
        ///  Persists the session to its backing store.
        /// </summary>
        Task Persist();

        /// <summary>
        ///  Terminates the session.
        /// </summary>
        void End();
    }
}
