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

namespace iSynaptic.Commons
{
    public class ReadWriteQualifier<TQualifier, TItem> : ReadableQualifier<TQualifier, TItem>, IReadWriteQualifier<TQualifier, TItem>
    {
        private readonly Action<TQualifier, TItem> _SetByQualifier = null;

        public ReadWriteQualifier(Func<TQualifier, TItem> getByQualifier, Action<TQualifier, TItem> setByQualifier)
            : this(getByQualifier, setByQualifier, null)
        {
        }

        public ReadWriteQualifier(Func<TQualifier, TItem> getByQualifier, Action<TQualifier, TItem> setByQualifier, Func<IEnumerable<TQualifier>> knownQualifiers)
            : base(getByQualifier, knownQualifiers)
        {
           _SetByQualifier = Guard.NotNull(setByQualifier, "setByQualifier");
        }

        TItem IReadWriteQualifier<TQualifier, TItem>.this[TQualifier qualifier]
        {
            get { return base[qualifier]; }
            set { _SetByQualifier(qualifier, value); }
        }
    }
}