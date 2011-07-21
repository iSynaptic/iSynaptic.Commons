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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons
{
    public class CompositeDisposable : ICollection<IDisposable>, IDisposable
    {
        private readonly List<IDisposable> _Disposables = new List<IDisposable>();

        public T Add<T>(T disposable) where T : IDisposable
        {
            Guard.NotNull(disposable, "disposable");

            _Disposables.Add(disposable);
            return disposable;
        }

        public void Add(IDisposable item)
        {
            _Disposables.Add(item);
        }

        public bool Contains(IDisposable disposable)
        {
            Guard.NotNull(disposable, "disposable");
            return _Disposables.Contains(disposable);
        }

        public void CopyTo(IDisposable[] array, int arrayIndex)
        {
            _Disposables.CopyTo(array, arrayIndex);
        }

        public bool Remove(IDisposable item)
        {
            return _Disposables.Remove(item);
        }

        public int Count
        {
            get { return _Disposables.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public void Clear()
        {
            _Disposables.Clear();
        }

        public IEnumerator<IDisposable> GetEnumerator()
        {
            return _Disposables.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Dispose()
        {
            var exceptions = new List<Exception>();

            foreach(var disposable in _Disposables)
            {
                try { disposable.Dispose(); }
                catch (Exception ex) { exceptions.Add(ex); }
            }

            Clear();

            if (exceptions.Count > 0)
                throw new AggregateException("Exception(s) occurred during disposal.", exceptions);
        }

    }
}
