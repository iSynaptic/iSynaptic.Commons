using System;
using System.Collections.Generic;

namespace iSynaptic.Commons.Collections.Generic
{
    public static class DictionaryExtensions
    {
        public static ReadOnlyDictionary<TKey, TValue> ToReadOnlyDictionary<TKey, TValue>(this IDictionary<TKey, TValue> @this)
        {
            Guard.NotNull(@this, "@this");
            return @this as ReadOnlyDictionary<TKey, TValue> ?? new ReadOnlyDictionary<TKey, TValue>(@this);
        }

        public static Maybe<TValue> TryGetValue<TKey, TValue>(this IDictionary<TKey, TValue> @this, TKey key)
        {
            Guard.NotNull(@this, "@this");

            TValue retreivedValue = default(TValue);

            return @this
                .ToMaybe()
                .Where(x => x.TryGetValue(key, out retreivedValue))
                .Select(x => retreivedValue);
        }
    }
}
