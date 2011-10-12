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
    public class WeakReference<T> : WeakReference, IEquatable<WeakReference<T>>
        where T : class
    {
        public static readonly WeakReference<T> Null = new WeakNullReference();

        private class WeakNullReference : WeakReference<T>
        {
            public WeakNullReference() : base(null, null) { }

            public override bool IsAlive
            {
                get { return true; }
            }
        }

        public static WeakReference<T> Create(T target)
        {
            return Create(target, EqualityComparer<T>.Default);
        }

        public static WeakReference<T> Create(T target, IEqualityComparer<T> comparer)
        {
            if (target == null)
                return Null;

            Guard.NotNull(comparer, "comparer");
            return new WeakReference<T>(target, comparer);
        }

        protected WeakReference(T target, IEqualityComparer<T> comparer)
            : base(target, false)
        {
            HashCode = target != null
                ? comparer.GetHashCode(target)
                : 0;
        }

        public new T Target
        {
            get { return (T)base.Target; }
        }

        public int HashCode { get; private set; }

        public bool Equals(WeakReference<T> other)
        {
            return Maybe
                .Value(true)
                .Unless(x => ReferenceEquals(this, Null))
                .Where(x => HashCode == other.HashCode)
                .Where(x => TryGetTarget().Equals(other.TryGetTarget()))
                .Or(() => ReferenceEquals(other, Null))
                .ValueOrDefault();
        }

        public Maybe<T> TryGetTarget()
        {
            return Maybe.Defer(() => Target)
                .Where(x => IsAlive);
        }

        public override bool Equals(object obj)
        {
            return !ReferenceEquals(obj, null) && Equals(obj as WeakReference<T>);
        }

        public override int GetHashCode()
        {
            return HashCode;
        }
    }
}
