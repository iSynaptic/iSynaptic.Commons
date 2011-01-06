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

        protected override IEqualityComparer<WeakReference<TKey>> BuildComparer(IEqualityComparer<TKey> comparer)
        {
            return new WeakKeyComparer<TKey>(comparer);
        }

        protected override WeakReference<TKey> WrapKey(TKey key, IEqualityComparer<TKey> comparer)
        {
            return new WeakKeyReference<TKey>(key, comparer);
        }

        protected override bool UnwrapKey(WeakReference<TKey> key, ref TKey destination)
        {
            return UnwrapWeakReference(key, ref destination);
        }

        protected override TValue WrapValue(TValue value)
        {
            return value;
        }

        protected override bool UnwrapValue(TValue value, ref TValue destination)
        {
            destination = value;
            return true;
        }
    }
}
