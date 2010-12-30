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
