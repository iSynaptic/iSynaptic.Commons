using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons.Collections.Generic
{
    public abstract class BaseReadOnlyDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private readonly ICollection<TKey> _Keys = null;
        private readonly ICollection<TValue> _Values = null;

        protected BaseReadOnlyDictionary()
        {
            _Keys = this.ToProjectedCollection(x => x.Key);
            _Values = this.ToProjectedCollection(x => x.Value);
        }

        public abstract int Count { get; }
        public abstract bool ContainsKey(TKey key);
        public abstract bool TryGetValue(TKey key, out TValue value);
        public abstract IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator();

        void ICollection<KeyValuePair<TKey, TValue>>.Clear()
        {
            throw new NotSupportedException("Dictionary is read-only.");
        }

        void IDictionary<TKey, TValue>.Add(TKey key, TValue value)
        {
            throw new NotSupportedException("Dictionary is read-only.");
        }

        bool IDictionary<TKey, TValue>.Remove(TKey key)
        {
            throw new NotSupportedException("Dictionary is read-only.");
        }

        public virtual bool IsReadOnly
        {
            get { return true; }
        }

        public ICollection<TKey> Keys
        {
            get { return _Keys; }
        }

        public ICollection<TValue> Values
        {
            get { return _Values; }
        }

        public TValue this[TKey key]
        {
            get
            {
                TValue value;

                if (!TryGetValue(key, out value))
                    throw new KeyNotFoundException();

                return value;
            }
            set
            {
                throw new NotSupportedException("Dictionary is read-only.");
            }
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            throw new NotSupportedException("Dictionary is read-only.");
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            TValue value;

            if (!TryGetValue(item.Key, out value))
                return false;

            return EqualityComparer<TValue>.Default.Equals(value, item.Value);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] destination, int index)
        {
            ((IEnumerable<KeyValuePair<TKey, TValue>>)this).CopyTo(destination, index);
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            throw new NotSupportedException("Dictionary is read-only.");
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
