using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons.Collections.Generic
{
    public class WeakDictionary<TKey, TValue> : BaseWeakDictionary<TKey, TValue, WeakReference<TKey>, WeakReference<TValue>>
        where TKey : class
        where TValue : class
    {
        public WeakDictionary(int capacity = 0, IEqualityComparer<TKey> comparer = null)
            : base(capacity, comparer)
        {
        }

        protected override WeakReference<TKey> WrapKey(TKey key, IEqualityComparer<TKey> comparer)
        {
            return WeakReference<TKey>.Create(key, comparer);
        }

        protected override Maybe<TKey> UnwrapKey(WeakReference<TKey> key)
        {
            return key.TryGetTarget();
        }

        protected override WeakReference<TValue> WrapValue(TValue value)
        {
            return WeakReference<TValue>.Create(value);
        }

        protected override Maybe<TValue> UnwrapValue(WeakReference<TValue> value)
        {
            return value.TryGetTarget();
        }
    } 
}
