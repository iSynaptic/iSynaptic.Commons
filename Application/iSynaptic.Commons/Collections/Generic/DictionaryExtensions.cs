using System;
using System.Collections.Generic;

namespace iSynaptic.Commons.Collections.Generic
{
    public static class DictionaryExtensions
    {
        public static ReadOnlyDictionary<TKey, TValue> ToReadOnlyDictionary<TKey, TValue>(this IDictionary<TKey, TValue> self)
        {
            Guard.NotNull(self, "self");

            if (self is ReadOnlyDictionary<TKey, TValue>)
                return self as ReadOnlyDictionary<TKey, TValue>;

            return new ReadOnlyDictionary<TKey, TValue>(self);
        }
    }
}
