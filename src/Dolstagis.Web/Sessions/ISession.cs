using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
