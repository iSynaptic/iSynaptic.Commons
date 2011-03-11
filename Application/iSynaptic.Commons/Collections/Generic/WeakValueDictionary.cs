using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons.Collections.Generic
{
    public sealed class WeakValueDictionary<TKey, TValue> : BaseWeakDictionary<TKey, TValue, TKey, WeakReference<TValue>>
        where TValue : class
    {
        public WeakValueDictionary()
            : this(0) { }

        public WeakValueDictionary(int capacity)
            : this(capacity, null) { }

        public WeakValueDictionary(IEqualityComparer<TKey> comparer)
            : this(0, comparer) { }

        public WeakValueDictionary(int capacity, IEqualityComparer<TKey> comparer)
            : base(capacity, comparer)
        {
        }

        protected override TKey WrapKey(TKey key, IEqualityComparer<TKey> comparer)
        {
            return key;
        }

        protected override Maybe<TKey> UnwrapKey(TKey key)
        {
            return key;
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
