using System;
using System.Collections.Generic;
using System.Text;

namespace iSynaptic.Commons.Collections.Generic
{
    public class ValueEqualityComparer<T> : EqualityComparer<T>, IEqualityComparer<T>
    {
        public static bool CanCompare()
        {
            return false;
        }

        public override bool Equals(T x, T y)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode(T obj)
        {
            throw new NotImplementedException();
        }
    }
}
