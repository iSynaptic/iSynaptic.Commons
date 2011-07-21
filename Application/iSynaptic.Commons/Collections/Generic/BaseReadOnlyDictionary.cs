// The MIT License
// 
// Copyright (c) 2011 Jordan E. Terrell
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iSynaptic.Commons.Linq;

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

        protected virtual T OnWriteOperation<T>()
        {
             throw new NotSupportedException("Dictionary is read-only.");
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Clear()
        {
            OnWriteOperation<bool>();
        }

        void IDictionary<TKey, TValue>.Add(TKey key, TValue value)
        {
            OnWriteOperation<bool>();
        }

        bool IDictionary<TKey, TValue>.Remove(TKey key)
        {
            return OnWriteOperation<bool>();
        }

        public bool IsReadOnly
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
                return this.TryGetValue(key)
                    .ThrowOnNoValue(new KeyNotFoundException())
                    .Value;
            }
            set
            {
                OnWriteOperation<bool>();
            }
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            OnWriteOperation<bool>();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return this.TryGetValue(item.Key)
                .Select(x => EqualityComparer<TValue>.Default.Equals(x, item.Value))
                .ValueOrDefault(false);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] destination, int index)
        {
            ((IEnumerable<KeyValuePair<TKey, TValue>>)this).CopyTo(destination, index);
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            return OnWriteOperation<bool>();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
