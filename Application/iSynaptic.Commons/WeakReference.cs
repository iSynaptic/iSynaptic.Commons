using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons
{
    public class WeakReference<T> : WeakReference where T : class
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
    }
}
