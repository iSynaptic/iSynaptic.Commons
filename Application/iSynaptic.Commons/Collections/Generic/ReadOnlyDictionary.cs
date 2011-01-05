using System;
using System.Collections.Generic;
using System.Collections;

namespace iSynaptic.Commons.Collections.Generic
{
    public class ReadOnlyDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private readonly IDictionary<TKey, TValue> _InnerDictionary = null;

        public ReadOnlyDictionary(IDictionary<TKey, TValue> innerDictionary)
        {
            Guard.NotNull(innerDictionary, "innerDictionary");

            _InnerDictionary = innerDictionary;
        }

        #region IDictionary<TKey,TValue> Members

        void IDictionary<TKey, TValue>.Add(TKey key, TValue value)
        {
            throw new NotSupportedException("Dictionary is read-only.");
        }

        public bool ContainsKey(TKey key)
        {
            return _InnerDictionary.ContainsKey(key);
        }

        public ICollection<TKey> Keys
        {
            get { return _InnerDictionary.Keys; }
        }

        bool IDictionary<TKey, TValue>.Remove(TKey key)
        {
            throw new NotSupportedException("Dictionary is read-only.");
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return _InnerDictionary.TryGetValue(key, out value);
        }

        public ICollection<TValue> Values
        {
            get { return _InnerDictionary.Values; }
        }

        public TValue this[TKey key]
        {
            get { return _InnerDictionary[key]; }
            set { throw new NotSupportedException("Dictionary is read-only."); }
        }

        #endregion

        #region ICollection<KeyValuePair<TKey,TValue>> Members

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            throw new NotSupportedException("Dictionary is read-only.");
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Clear()
        {
            throw new NotSupportedException("Dictionary is read-only.");
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return _InnerDictionary.Contains(item);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            _InnerDictionary.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _InnerDictionary.Count; }
        }

        public bool IsReadOnly
        {
            get { return true; }
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            throw new NotSupportedException("Dictionary is read-only.");
        }

        #endregion

        #region IEnumerable<KeyValuePair<TKey,TValue>> Members

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _InnerDictionary.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_InnerDictionary).GetEnumerator();
        }

        #endregion
    }
}
