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
                .Extract();
        }

        public Maybe<T> TryGetTarget()
        {
            return Maybe.Value(() => Target)
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
