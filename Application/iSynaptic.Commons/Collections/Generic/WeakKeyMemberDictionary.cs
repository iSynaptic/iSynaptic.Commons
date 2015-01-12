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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons.Collections.Generic
{
    public sealed class WeakKeyMemberDictionary<TKey, TKeyMember, TValue> : BaseWeakDictionary<TKey, TValue, TKey, TValue> where TKeyMember : class
    {
        private readonly Func<TKey, WeakReference<TKeyMember>> _MemberSelector = null;

        public WeakKeyMemberDictionary(Func<TKey, WeakReference<TKeyMember>> memberSelector, int capacity = 0, IEqualityComparer<TKey> comparer = null)
            : base(capacity, comparer)
        {
             _MemberSelector = memberSelector;
        }

        protected override TKey WrapKey(TKey key, IEqualityComparer<TKey> comparer)
        {
            return key;
        }

        protected override Maybe<TKey> UnwrapKey(TKey key)
        {
            return _MemberSelector(key)
                .ToMaybe()
                .SelectMaybe(x => x.TryGetTarget())
                .Select(x => key);
        }

        protected override TValue WrapValue(TValue value)
        {
            return value;
        }

        protected override Maybe<TValue> UnwrapValue(TValue value)
        {
            return value.ToMaybe();
        }
    }
}
