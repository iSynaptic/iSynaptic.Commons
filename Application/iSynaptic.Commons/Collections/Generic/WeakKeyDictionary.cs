using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons.Collections.Generic
{
    public sealed class WeakKeyDictionary<TKey, TValue> : BaseWeakDictionary<TKey, TValue, WeakReference<TKey>, TValue>
        where TKey : class
    {
        public WeakKeyDictionary()
            : this(0) { }

        public WeakKeyDictionary(int capacity)
            : this(capacity, null) { }

        public WeakKeyDictionary(IEqualityComparer<TKey> comparer)
            : this(0, comparer) { }

        public WeakKeyDictionary(int capacity, IEqualityComparer<TKey> comparer)
            : base(capacity, comparer)
        {
        }

        protected override WeakReference<TKey> WrapKey(TKey key, IEqualityComparer<TKey> comparer)
        {
            return WeakReference<TKey>.Create(key, comparer);
        }

        protected override Maybe<TKey> UnwrapKey(WeakReference<TKey> key)
        {
            return UnwrapWeakReference(key);
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
