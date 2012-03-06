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

namespace iSynaptic.Commons
{
    public class KeyedReaderWriter<TKey, TValue> : IKeyedReaderWriter<TKey, TValue>
    {
        private readonly Func<TKey, TValue> _Getter = null;
        private readonly Func<TKey, TValue, bool> _Setter = null;
        private readonly Func<IEnumerable<TKey>> _Keys = null;

        public KeyedReaderWriter(Func<TKey, TValue> getter, Func<TKey, TValue, bool> setter, Func<IEnumerable<TKey>> keys)
        {
            _Getter = getter;
            _Setter = setter;
            _Keys = keys;
        }

        public TValue Get(TKey key)
        {
            return _Getter != null 
                ? _Getter(key) 
                : default(TValue);
        }

        public bool Set(TKey key, TValue value)
        {
            return _Setter != null && _Setter(key, value);
        }

        public IEnumerable<TKey> GetKeys()
        {
            return _Keys != null
                ? _Keys() 
                : Enumerable.Empty<TKey>();
        }
    }
}
