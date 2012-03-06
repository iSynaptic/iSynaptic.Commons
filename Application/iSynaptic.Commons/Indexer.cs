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
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace iSynaptic.Commons
{
    public static class Indexer
    {
        private class KeyedIndexer<TIndex, TValue> : IReadableIndexer<TIndex, TValue>, IReadWriteIndexer<TIndex, TValue>
        {
            private readonly IKeyedReaderWriter<TIndex, TValue> _KeyedReaderWriter;

            public KeyedIndexer(IKeyedReaderWriter<TIndex, TValue> keyedReaderWriter)
            {
                _KeyedReaderWriter = keyedReaderWriter;
            }

            TValue IReadableIndexer<TIndex, TValue>.this[TIndex index]
            {
                get { return _KeyedReaderWriter.Get(index); }
            }

            TValue IReadWriteIndexer<TIndex, TValue>.this[TIndex index]
            {
                get { return _KeyedReaderWriter.Get(index); }
                set { _KeyedReaderWriter.Set(index, value); }
            }

            public IEnumerator<TIndex> GetEnumerator()
            {
                return _KeyedReaderWriter.GetKeys()
                    .GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        public static IReadableIndexer<TIndex, TValue> ReadOnly<TIndex, TValue>(Func<TIndex, TValue> getter, Func<IEnumerable<TIndex>> knownIndexes = null)
        {
            Guard.NotNull(getter, "getter");
            return new KeyedIndexer<TIndex, TValue>(new KeyedReaderWriter<TIndex, TValue>(getter, null, knownIndexes));
        }

        public static IReadWriteIndexer<TIndex, TValue> ReadWrite<TIndex, TValue>(Func<TIndex, TValue> getter, Action<TIndex, TValue> setter, Func<IEnumerable<TIndex>> knownIndexes = null)
        {
            Guard.NotNull(getter, "getter");
            Guard.NotNull(setter, "setter");

            return new KeyedIndexer<TIndex, TValue>(new KeyedReaderWriter<TIndex, TValue>(getter, (i, v) => { setter(i, v); return true; }, knownIndexes));
        }

        public static IReadWriteIndexer<TIndex, TValue> FromKeyedReaderWriter<TIndex, TValue>(IKeyedReaderWriter<TIndex, TValue> keyedReaderWriter)
        {
            return new KeyedIndexer<TIndex, TValue>(Guard.NotNull(keyedReaderWriter, "keyedReaderWriter"));
        }
    }
}
