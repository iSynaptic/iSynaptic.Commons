using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons
{
    public sealed class WeakKeyReference<T> : WeakReference<T> where T : class
    {
        public WeakKeyReference(T key, IEqualityComparer<T> comparer)
            : base(key)
        {
            Guard.NotNull(comparer, "comparer");
            HashCode = comparer.GetHashCode(key);
        }

        public int HashCode { get; private set; }
    }
}
