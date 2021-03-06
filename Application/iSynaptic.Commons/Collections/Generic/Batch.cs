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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons.Collections.Generic
{
    public static class Batch
    {
        public static Batch<T> Create<T>(IEnumerable<T> batch, int index, int itemIndex)
        {
            return new Batch<T>(batch, index, itemIndex);
        }
    }

    public class Batch<T> : IEnumerable<T>
    {
        private readonly T[] _batch;

        public Batch(IEnumerable<T> batch, int index, int itemIndex)
        {
            _batch = Guard.NotNull(batch, "batch")
                .ToArray();

            if(index < 0) throw new ArgumentOutOfRangeException("index", "Index must be not be negative.");
            if(itemIndex < 0) throw new ArgumentOutOfRangeException("itemIndex", "Item index must not be negative.");

            Count = _batch.Length;
            Index = index;
            ItemIndex = itemIndex;
        }

        public int Index { get; private set; }
        public int ItemIndex { get; private set; }
        public int Count { get; private set; }

        public T this[int index]
        {
            get { return _batch[index]; }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)_batch).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
