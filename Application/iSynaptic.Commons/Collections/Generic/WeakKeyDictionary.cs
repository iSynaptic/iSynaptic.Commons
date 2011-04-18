using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons.Collections.Generic
{
    public sealed class WeakKeyDictionary<TKey, TValue> : BaseWeakDictionary<TKey, TValue, WeakReference<TKey>, TValue>
        where TKey : class
    {
        public WeakKeyDictionary(int capacity = 0, IEqualityComparer<TKey> comparer = null, Action<Maybe<TKey>, Maybe<TValue>> onGarbagePurge = null)
            : base(capacity, comparer, onGarbagePurge)
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

        protected override TValue WrapValue(TValue value)
        {
            return value;
        }

        protected override Maybe<TValue> UnwrapValue(TValue value)
        {
            return value;
        }
    }
}
