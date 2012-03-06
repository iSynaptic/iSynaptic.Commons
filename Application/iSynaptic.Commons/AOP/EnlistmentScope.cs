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
using System.Threading;
using iSynaptic.Commons.Collections.Generic;

namespace iSynaptic.Commons.AOP
{
    public abstract class EnlistmentScope<TItem, TScope> : Scope<TScope>, IEnlistmentScope<TItem>
        where TScope : EnlistmentScope<TItem, TScope>
    {
        private List<TItem> _Items = null;

        protected EnlistmentScope(ScopeBounds bounds, ScopeNesting nesting) : base(bounds, nesting)
        {
        }

        public virtual bool IsEnlisted(TItem item)
        {
            if (Disposed)
                throw new ObjectDisposedException(GetType().Name);

            return Items.Contains(item);
        }

        public void Enlist(params TItem[] items)
        {
            if (Disposed)
                throw new ObjectDisposedException(GetType().Name);

            Enlist((IEnumerable<TItem>)items);
        }

        public virtual void Enlist(IEnumerable<TItem> items)
        {
            if (Disposed)
                throw new ObjectDisposedException(GetType().Name);

            Items.AddRange(items);
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (Disposed != true)
                    Items.Clear();
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        protected virtual ICollection<TItem> Items
        {
            get
            {
                if (_Items == null)
                    Interlocked.CompareExchange(ref _Items, new List<TItem>(), null);

                return _Items;
            }
        }
    }
}
