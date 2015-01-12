// The MIT License
// 
// Copyright (c) 2012-2015 Jordan E. Terrell
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
    public abstract class BaseWeakDictionary<TKey, TValue, TWrappedKey, TWrappedValue> : BaseDictionary<TKey, TValue>, IWeakDictionary<TKey, TValue>
    {
        private readonly IEqualityComparer<TKey> _Comparer;
        private readonly Dictionary<TWrappedKey, TWrappedValue> _Dictionary;

        protected BaseWeakDictionary(int capacity, IEqualityComparer<TKey> comparer)
        {
            _Comparer = comparer ?? EqualityComparer<TKey>.Default;

            var wrappedKeyComparer = comparer as IEqualityComparer<TWrappedKey>;

            _Dictionary = wrappedKeyComparer != null
                ? new Dictionary<TWrappedKey, TWrappedValue>(capacity, wrappedKeyComparer)
                : new Dictionary<TWrappedKey, TWrappedValue>(capacity);
       }

        protected abstract TWrappedKey WrapKey(TKey key, IEqualityComparer<TKey> comparer);
        protected abstract Maybe<TKey> UnwrapKey(TWrappedKey key);
        protected abstract TWrappedValue WrapValue(TValue value);
        protected abstract Maybe<TValue> UnwrapValue(TWrappedValue value);

        public override int Count
        {
            get { return _Dictionary.Count; }
        }

        public override void Add(TKey key, TValue value)
        {
            var weakKey = WrapKey(key, _Comparer);
            var weakValue = WrapValue(value);

            _Dictionary.Add(weakKey, weakValue);
        }

        public override bool ContainsKey(TKey key)
        {
            return _Dictionary.ContainsKey(WrapKey(key, _Comparer));
        }

        public override bool Remove(TKey key)
        {
            return _Dictionary.Remove(WrapKey(key, _Comparer));
        }

        public override bool TryGetValue(TKey key, out TValue value)
        {
            var result = _Dictionary
                .TryGetValue(WrapKey(key, _Comparer))
                .SelectMaybe(UnwrapValue);

            value = result.ValueOrDefault();
            return result.HasValue;
        }

        protected override void SetValue(TKey key, TValue value)
        {
            var weakKey = WrapKey(key, _Comparer);
            _Dictionary[weakKey] = WrapValue(value);
        }

        public override void Clear()
        {
            _Dictionary.Clear();
        }

        public override IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _Dictionary
                .Select(pair => UnwrapKey(pair.Key).Join(UnwrapValue(pair.Value), KeyValuePair.Create))
                .Squash()
                .GetEnumerator();
        }

        public void PurgeGarbage(Action<Maybe<TKey>, Maybe<TValue>> withPurgedPair)
        {
            var itemsToPurge = _Dictionary
                .Where(pair => !UnwrapKey(pair.Key).Join(UnwrapValue(pair.Value)).HasValue)
                .ToArray();

            if (withPurgedPair != null)
            {
                foreach (var item in itemsToPurge)
                    withPurgedPair(UnwrapKey(item.Key), UnwrapValue(item.Value));
            }

            _Dictionary.Remove(itemsToPurge);
        }
    } 
}
