﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace iSynaptic.Commons.Collections.Generic
{
    public abstract class BaseDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private readonly ICollection<TKey> _Keys = null;
        private readonly ICollection<TValue> _Values = null;

        protected BaseDictionary()
        {
            _Keys = this.ToProjectedCollection(x => x.Key);
            _Values = this.ToProjectedCollection(x => x.Value);
        }

        public abstract int Count { get; }
        public abstract void Clear();
        public abstract void Add(TKey key, TValue value);
        public abstract bool ContainsKey(TKey key);
        public abstract bool Remove(TKey key);
        public abstract bool TryGetValue(TKey key, out TValue value);
        public abstract IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator();
        protected abstract void SetValue(TKey key, TValue value);

        public virtual bool IsReadOnly
        {
            get { return false; }
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
                SetValue(key, value);
            }
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
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

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return Remove(item.Key);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    } 
}
