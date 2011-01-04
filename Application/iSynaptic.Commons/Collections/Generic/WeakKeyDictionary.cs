using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons.Collections.Generic
{
    public sealed class WeakKeyDictionary<TKey, TValue> : WeakDictionary<TKey, TValue>
        where TKey : class
        where TValue : class
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

        protected override object WrapValue(TValue value)
        {
            return value;
        }

        protected override bool UnwrapValue(object value, ref TValue destination)
        {
            destination = (TValue) value;
            return true;
        }
    }
}
