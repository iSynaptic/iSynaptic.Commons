using System;
using System.Collections.Generic;
using System.Text;
using iSynaptic.Commons.Collections.Generic;

namespace iSynaptic.Commons.Extensions
{
    public static class DictionaryExtensions
    {
        public static ReadOnlyDictionary<TKey, TValue> ToReadOnlyDictionary<TKey, TValue>(this IDictionary<TKey, TValue> self)
        {
            if (self == null)
                throw new ArgumentNullException("self");

            if (self is ReadOnlyDictionary<TKey, TValue>)
                return self as ReadOnlyDictionary<TKey, TValue>;

            return new ReadOnlyDictionary<TKey, TValue>(self);
        }
    }
}
