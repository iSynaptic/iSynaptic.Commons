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
            public WeakNullReference() : base(null) { }

            public override bool IsAlive
            {
                get { return true; }
            }
        }

        public static WeakReference<T> Create(T target)
        {
            if (target == null)
                return Null;

            return new WeakReference<T>(target);
        }

        protected WeakReference(T target)
            : base(target, false) { }

        public new T Target
        {
            get { return (T)base.Target; }
        }
    }
}
