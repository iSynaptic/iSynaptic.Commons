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
using System.Threading;

namespace iSynaptic.Commons.Collections.Generic
{
    public class MultiMap<TKey, TValue>
    {
        private readonly Func<TKey, ICollection<TValue>> _CollectionFactory = null;

        public MultiMap()
            : this(new Dictionary<TKey, ICollection<TValue>>())
        {
        }

        public MultiMap(IDictionary<TKey, ICollection<TValue>> dictionary)
            : this(GetDictionaryBackedCollectionFactory(dictionary))
        {
        }

        public MultiMap(Func<TKey, ICollection<TValue>> collectionFactory)
        {
            _CollectionFactory = Guard.NotNull(collectionFactory, "collectionFactory");
        }

        private static Func<TKey, ICollection<TValue>> GetDictionaryBackedCollectionFactory(IDictionary<TKey, ICollection<TValue>> dictionary)
        {
            Guard.NotNull(dictionary, "dictionary");

            if(dictionary.IsReadOnly)
                throw new ArgumentException("Dictionary must not be read-only.", "dictionary");

            return x => dictionary
                .TryGetValue(x)
                .Or((Func<ICollection<TValue>>)(() =>
                {
                    var values = new List<TValue>();
                    dictionary.Add(x, values);

                    return values;
                }))
                .Value;
        }

        public void Add(TKey key, TValue value)
        {
            var collection = GetCollection(key);
            collection.Add(value);
        }

        public void AddRange(TKey key, IEnumerable<TValue> values)
        {
            Guard.NotNull(values, "values");
            
            var collection = GetCollection(key);
            collection.AddRange(values);
        }

        public void Remove(TKey key, TValue value)
        {
            var collection = GetCollection(key);
            collection.Remove(value);
        }

        public ICollection<TValue> this[TKey key]
        {
            get { return GetCollection(key); }
        }

        protected virtual ICollection<TValue> GetCollection(TKey key)
        {
            return _CollectionFactory(key);
        }
    }
}
