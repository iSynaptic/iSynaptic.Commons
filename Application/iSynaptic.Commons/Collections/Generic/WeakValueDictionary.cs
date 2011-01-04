using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons.Collections.Generic
{
    public sealed class WeakValueDictionary<TKey, TValue> : WeakDictionary<TKey, TValue>
        where TKey : class
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

        protected override object WrapKey(TKey key)
        {
            return key;
        }

        protected override bool UnwrapKey(object key, ref TKey destination)
        {
            destination = (TKey) key;
            return true;
        }
    }
}
