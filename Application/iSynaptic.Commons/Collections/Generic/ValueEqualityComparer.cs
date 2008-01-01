using System;
using System.Collections.Generic;
using System.Text;

namespace iSynaptic.Commons.Collections.Generic
{
    public class ValueEqualityComparer<T> : EqualityComparer<T>, IEqualityComparer<T>
    {
        private static Func<T, T, bool> _Equals = null;
        private static Func<T, int> _GetHashCode = null;

        private static void EnsureMethodsExists()
        {
            if (_Equals != null && _GetHashCode != null)
                return;
        }

        public override bool Equals(T x, T y)
        {
            EnsureMethodsExists();

            return _Equals(x, y);
        }

        public override int GetHashCode(T obj)
        {
            EnsureMethodsExists();

            return _GetHashCode(obj);
        }
    }
}
