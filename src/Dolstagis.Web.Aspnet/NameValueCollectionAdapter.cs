using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.Aspnet
{
    public class NameValueCollectionAdapter : IDictionary<string, string[]>
    {
        private NameValueCollection inner;

        public NameValueCollectionAdapter(NameValueCollection inner)
        {
            this.inner = inner;
        }

        public void Add(string key, string[] value)
        {
            this.inner.Remove(key);
            foreach (string s in value)
            {
                this.inner.Add(key, s);
            }
        }

        public bool ContainsKey(string key)
        {
            return this.inner.GetValues(key) != null;
        }

        public ICollection<string> Keys
        {
            get { return this.inner.AllKeys; }
        }

        public bool Remove(string key)
        {
            bool exists = this.ContainsKey(key);
            this.inner.Remove(key);
            return exists;
        }

        public bool TryGetValue(string key, out string[] value)
        {
            value = this.inner.GetValues(key);
            return value != null;
        }

        public ICollection<string[]> Values
        {
            get {
                return this.inner.Keys.Cast<string>()
                    .Select(x => this.inner.GetValues(x)).ToList();
            }
        }

        public string[] this[string key]
        {
            get
            {
                if (key == null) throw new ArgumentNullException();
                var result = this.inner.GetValues(key);
                if (result == null) throw new KeyNotFoundException();
                return result;
            }
            set
            {
                this.Add(key, value);
            }
        }

        public void Add(KeyValuePair<string, string[]> item)
        {
            this.Add(item.Key, item.Value);
        }

        public void Clear()
        {
            this.inner.Clear();
        }

        public bool Contains(KeyValuePair<string, string[]> item)
        {
            throw new NotSupportedException();
        }

        public void CopyTo(KeyValuePair<string, string[]>[] array, int arrayIndex)
        {
            throw new NotSupportedException();
        }

        public int Count
        {
            get { return this.inner.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(KeyValuePair<string, string[]> item)
        {
            var result = this.ContainsKey(item.Key);
            this.inner.Remove(item.Key);
            return result;
        }

        public IEnumerator<KeyValuePair<string, string[]>> GetEnumerator()
        {
            var enumerable =
                from key in this.inner.Keys.Cast<string>()
                select new KeyValuePair<string, string[]>(key, this[key]);
            return enumerable.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            var enumerable =
                from key in this.inner.Keys.Cast<string>()
                select new KeyValuePair<string, string[]>(key, this[key]);
            return enumerable.GetEnumerator();
        }
    }
}
