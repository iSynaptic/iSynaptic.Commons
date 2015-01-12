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
using iSynaptic.Commons.Linq;

namespace iSynaptic.Commons.Collections.Generic
{
    public class ProjectionCollection<TSourceItem, TProjectedItem> : ICollection<TProjectedItem>
    {
        private static readonly NotSupportedException ReadOnlyException = new NotSupportedException("Mutating a readonly collection is not allowed.");

        private readonly ICollection<TSourceItem> _Underlying = null;
        private readonly Func<TSourceItem, TProjectedItem> _Selector = null;

        public ProjectionCollection(ICollection<TSourceItem> underlying, Func<TSourceItem, TProjectedItem> selector)
        {
            _Underlying = Guard.NotNull(underlying, "underlying");
            _Selector = Guard.NotNull(selector, "selector");
        }

        public IEnumerator<TProjectedItem> GetEnumerator()
        {
            return _Underlying.Select(_Selector).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        void ICollection<TProjectedItem>.Add(TProjectedItem item)
        {
            throw ReadOnlyException;
        }

        void ICollection<TProjectedItem>.Clear()
        {
            throw ReadOnlyException;
        }

        public bool Contains(TProjectedItem item)
        {
            var predicate = item == null ? (Func<TSourceItem, bool>) (x => _Selector(x) == null) : (x => item.Equals(_Selector(x)));
            return _Underlying.Any(predicate);
        }

        public void CopyTo(TProjectedItem[] destination, int index)
        {
            _Underlying
                .Select(_Selector)
                .CopyTo(destination, index);
        }

        bool ICollection<TProjectedItem>.Remove(TProjectedItem item)
        {
            throw ReadOnlyException;
        }

        public int Count
        {
            get { return _Underlying.Count; }
        }

        public bool IsReadOnly
        {
            get { return true; }
        }
    }
}
