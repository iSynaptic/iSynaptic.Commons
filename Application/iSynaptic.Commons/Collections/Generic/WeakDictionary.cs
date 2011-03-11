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
        public WeakDictionary()
            : this(0) { }

        public WeakDictionary(int capacity)
            : this(capacity, null) { }

        public WeakDictionary(IEqualityComparer<TKey> comparer)
            : this(0, comparer) { }

        public WeakDictionary(int capacity, IEqualityComparer<TKey> comparer)
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

        protected override WeakReference<TValue> WrapValue(TValue value)
        {
            return WeakReference<TValue>.Create(value);
        }

        protected override Maybe<TValue> UnwrapValue(WeakReference<TValue> value)
        {
            return UnwrapWeakReference(value);
        }
    } 
}
