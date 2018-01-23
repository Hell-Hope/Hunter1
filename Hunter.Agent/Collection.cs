using System.Collections.Generic;

namespace Hunter.Agent
{
    public static class Collection
    {

        public static V TryGetClassValue<K, V>(this IDictionary<K, V> that, K key) where V : class
        {
            if (that.TryGetValue(key, out V value))
                return value;
            return null;
        }

        public static V? TryGetStructValue<K, V>(this IDictionary<K, V> that, K key) where V : struct
        {
            if (that.TryGetValue(key, out V value))
                return value;
            return null;
        }

    }
}
