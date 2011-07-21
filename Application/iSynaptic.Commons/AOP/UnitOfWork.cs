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

namespace iSynaptic.Commons.AOP
{
    public abstract class UnitOfWork<TItem, TUnitOfWork> : EnlistmentScope<TItem, TUnitOfWork>, IUnitOfWork<TItem>
        where TUnitOfWork : EnlistmentScope<TItem, TUnitOfWork>, IUnitOfWork<TItem>
    {
        protected UnitOfWork() : this(ScopeNesting.Allowed)
        {
        }

        protected UnitOfWork(ScopeNesting nesting) : base(ScopeBounds.Thread, nesting)
        {
        }

        protected abstract void Process(IEnumerable<TItem> items);

        public void Complete()
        {
            if(Disposed)
                throw new ObjectDisposedException(GetType().Name);

            if(Completed)
                throw new InvalidOperationException("Unit of work has already been completed.");

            Completed = true;
            Process(Items);
        }

        protected bool Completed { get; private set; }
    }
}
