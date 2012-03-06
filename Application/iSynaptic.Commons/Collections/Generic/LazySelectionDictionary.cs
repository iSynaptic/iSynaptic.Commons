// The MIT License
// 
// Copyright (c) 2012 Jordan E. Terrell
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

namespace iSynaptic.Commons.Collections.Generic
{
    public class LazySelectionDictionary<TKey, TValue> : BaseDictionary<TKey, TValue>
    {
        private readonly Func<TKey, Maybe<TValue>> _Selector;
        private readonly IDictionary<TKey, TValue> _Underlying = null;

        public LazySelectionDictionary(Func<TKey, Maybe<TValue>> selector)
            : this(selector, EqualityComparer<TKey>.Default)
        {
        }

        public LazySelectionDictionary(Func<TKey, Maybe<TValue>> selector, IEqualityComparer<TKey> comparer)
            : this(selector, new Dictionary<TKey, TValue>(comparer))
        {
        }

        public LazySelectionDictionary(Func<TKey, Maybe<TValue>> selector, IDictionary<TKey, TValue> underlying)
        {
            _Selector = Guard.NotNull(selector, "selector");

            Guard.NotNull(underlying, "underlying");
            
            if(underlying.IsReadOnly)
                throw new ArgumentException("Underlying dictionary must not be read-only.", "underlying");

            _Underlying = underlying;
        }

        public override int Count
        {
            get { return _Underlying.Count; }
        }

        public override void Clear()
        {
            _Underlying.Clear();
        }

        public override void Add(TKey key, TValue value)
        {
            _Underlying.Add(key, value);
        }

        public override bool Remove(TKey key)
        {
            return _Underlying.Remove(key);
        }

        public override bool ContainsKey(TKey key)
        {
            return this.TryGetValue(key).HasValue;
        }

        public override bool TryGetValue(TKey key, out TValue value)
        {
            var results = _Underlying
                .TryGetValue(key)
                .Or(() => _Selector(key).OnValue(x => _Underlying.Add(key, x)));

            value = results.ValueOrDefault();
            return results.HasValue;
        }

        public override IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _Underlying.GetEnumerator();
        }

        protected override void SetValue(TKey key, TValue value)
        {
            _Underlying[key] = value;
        }
    }
}
