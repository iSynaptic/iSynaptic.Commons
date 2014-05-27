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
using System.Text;
using System.Threading.Tasks;

namespace iSynaptic.Commons.Collections
{
    internal class WithHasCurrentEnumerator : IWithHasCurrentEnumerator
    {
        private readonly IEnumerator _underlying;
        private bool? _hasCurrent = null;

        public WithHasCurrentEnumerator(IEnumerator underlying)
        {
            _underlying = Guard.NotNull(underlying, "underlying");
        }

        public bool MoveNext()
        {
            _hasCurrent = _underlying.MoveNext();
            return HasCurrent;
        }

        public void Reset()
        {
            throw new NotSupportedException("Resetting the enumerator is not supported.");
        }

        public object Current { get { return _underlying.Current; } }

        public bool HasCurrent
        {
            get
            {
                if (!_hasCurrent.HasValue)
                    _hasCurrent = MoveNext();

                return _hasCurrent.Value;
            }
        }
    }
}
