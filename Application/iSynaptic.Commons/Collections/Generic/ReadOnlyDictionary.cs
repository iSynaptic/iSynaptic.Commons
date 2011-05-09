using System;
using System.Collections.Generic;
using System.Collections;

namespace iSynaptic.Commons.Collections.Generic
{
    public class ReadOnlyDictionary<TKey, TValue> : BaseReadOnlyDictionary<TKey, TValue>
    {
        private readonly IDictionary<TKey, TValue> _InnerDictionary = null;

        public ReadOnlyDictionary(IDictionary<TKey, TValue> innerDictionary)
        {
            _InnerDictionary = Guard.NotNull(innerDictionary, "innerDictionary");
        }

        public override int Count
        {
            get { return _InnerDictionary.Count; }
        }

        public override bool ContainsKey(TKey key)
        {
            return _InnerDictionary.ContainsKey(key);
        }

        public override bool TryGetValue(TKey key, out TValue value)
        {
            return _InnerDictionary.TryGetValue(key, out value);
        }

        public override IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _InnerDictionary.GetEnumerator();
        }
    }
}
