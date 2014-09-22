using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.Util
{
    public static class DictionaryExtensions
    {
        public static void Concat<TKey, TValue>
            (this IDictionary<TKey, TValue[]> target, IDictionary<TKey, TValue> other)
        {
            if (other == null) return;
            foreach (var key in other.Keys)
            {
                TValue[] existing;
                if (target.TryGetValue(key, out existing))
                {
                    target[key] = existing.Concat(Enumerable.Repeat(other[key], 1)).ToArray();
                }
                else
                {
                    target[key] = new TValue[] { other[key] };
                }
            }
        }

        public static void Concat<TKey, TValue>
            (this IDictionary<TKey, TValue[]> target, IDictionary<TKey, TValue[]> other)
        {
            if (other == null) return;
            foreach (var key in other.Keys)
            {
                TValue[] existing;
                if (target.TryGetValue(key, out existing))
                {
                    target[key] = existing.Concat(other[key]).ToArray();
                }
                else
                {
                    target[key] = other[key];
                }
            }
        }
    }
}
