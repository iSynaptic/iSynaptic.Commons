using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons
{
    public static class Equatable
    {
        public static bool IsEqualsTo<T>(this T self, T other)
        {
            return Equatable<T>.IsEqualsTo(self, other);
        }

        public static int ToHashCode<T>(this T self)
        {
            return Equatable<T>.ToHashCode(self);
        }
    }

    public static class Equatable<T>
    {
        private static Type _TargetType = null;

        static Equatable()
        {
            _TargetType = typeof (T);
        }

        public static bool IsEqualsTo(T source, T other)
        {
            throw new NotImplementedException();
        }

        public static int ToHashCode(T source)
        {
            throw new NotImplementedException();
        }
    }
}
