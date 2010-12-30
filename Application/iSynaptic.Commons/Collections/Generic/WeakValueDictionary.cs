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
