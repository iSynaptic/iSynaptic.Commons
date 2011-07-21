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
using System.Collections;

namespace iSynaptic.Commons.Collections.Generic
{
    internal class LookAheadEnumerator<T> : IEnumerator<LookAheadableValue<T>>
    {
        private readonly IEnumerator<T> _InnerEnumerator = null;
        private List<T> _LookAheadList = null;

        private T _Current = default(T);
        private bool _Disposed = false;

        public LookAheadEnumerator(IEnumerator<T> innerEnumerator)
        {
            _InnerEnumerator = innerEnumerator;
        }

        public void Dispose()
        {
            _InnerEnumerator.Dispose();
            _Disposed = true;
        }

        public bool MoveNext()
        {
            if (_Disposed)
                throw new ObjectDisposedException("IEnumerator<T>");

            if (LookAheadList.Count <= 0)
            {
                bool results = _InnerEnumerator.MoveNext();

                _Current = _InnerEnumerator.Current;
                return results;
            }

            _Current = LookAheadList[0];
            LookAheadList.RemoveAt(0);

            return true;
        }

        public void Reset()
        {
            if (_Disposed)
                throw new ObjectDisposedException("IEnumerator<T>");

            _InnerEnumerator.Reset();
            _LookAheadList = null;
        }

        public Maybe<T> LookAhead(int index)
        {
            if (_Disposed)
                throw new ObjectDisposedException("IEnumerator<T>");

            if (LookAheadList.Count >= (index + 1))
                return LookAheadList[index].ToMaybe();

            int itemsToEnumerate = (index + 1) - LookAheadList.Count;

            for (int itemsEnumerated = 0; itemsEnumerated < itemsToEnumerate; itemsEnumerated++)
            {
                bool results = _InnerEnumerator.MoveNext();
                if (results)
                {
                    LookAheadList.Add(_InnerEnumerator.Current);
                }
                else
                    return Maybe<T>.NoValue;
            }

            return LookAheadList[index].ToMaybe();
        }

        public LookAheadableValue<T> Current
        {
            get
            {
                if(_Disposed)
                    throw new ObjectDisposedException("IEnumerator<T>");

                return new LookAheadableValue<T>(_Current, this);
            }
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }

        protected List<T> LookAheadList
        {
            get { return _LookAheadList ?? (_LookAheadList = new List<T>()); }
        }
    }
}
