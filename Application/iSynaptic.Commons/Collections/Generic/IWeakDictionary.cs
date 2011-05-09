using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons.Collections.Generic
{
    public interface IWeakDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        void PurgeGarbage(Action<Maybe<TKey>, Maybe<TValue>> withPurgedPair);
    }
}
