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

        protected override WeakReference<TValue> WrapValue(TValue value)
        {
            return WeakReference<TValue>.Create(value);
        }

        protected override bool UnwrapValue(WeakReference<TValue> value, ref TValue destination)
        {
            return UnwrapWeakReference(value, ref destination);
        }
    } 
}
