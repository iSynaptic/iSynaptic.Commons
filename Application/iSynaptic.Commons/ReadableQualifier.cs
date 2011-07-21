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
using System.Text;

namespace iSynaptic.Commons
{
    public class ReadableQualifier<TQualifier, TItem> : IReadableQualifier<TQualifier, TItem>, IKnownQualifiers<TQualifier>
    {
        private readonly Func<TQualifier, TItem> _GetByQualifier = null;
        private readonly Func<IEnumerable<TQualifier>> _GetQualifiers = null;

        public ReadableQualifier(Func<TQualifier, TItem> getByQualifier) : this(getByQualifier, null)
        {
        }

        public ReadableQualifier(Func<TQualifier, TItem> getByQualifier, Func<IEnumerable<TQualifier>> knownQualifiers)
        {
            _GetByQualifier = Guard.NotNull(getByQualifier, "getByQualifier");
            _GetQualifiers = knownQualifiers ?? (() => Enumerable.Empty<TQualifier>());
        }

        public TItem this[TQualifier qualifier]
        {
            get { return _GetByQualifier(qualifier); }
        }

        public IEnumerable<TQualifier> GetQualifiers()
        {
            return _GetQualifiers();
        }
    }
}
