using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
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
        private static readonly object _SyncLock = null;

        private static Type _TargetType = null;
        private static Func<T, T, bool> _IsEqualToStrategy = null;
        //private static Func<T, int> _ToHashCodeStrategy = null;

        static Equatable()
        {
            _TargetType = typeof (T);
            _SyncLock = new object();
        }

        public static bool IsEqualsTo(T source, T other)
        {
            if(ReferenceEquals(source, null) && ReferenceEquals(other, null))
                return true;

            if(ReferenceEquals(source, null))
                return false;

            if(ReferenceEquals(other, null))
                return false;

            return IsEqualToStrategy(source, other);
        }

        public static int ToHashCode(T source)
        {
            if (ReferenceEquals(source, null))
                return 0;

            throw new NotImplementedException();
        }

        private static Func<T, T, bool> BuildIsEqualToStrategy()
        {
            return (x, y) => { throw new NotImplementedException(); };
        }

        private static Func<T, T, bool> IsEqualToStrategy
        {
            [ReflectionPermission(SecurityAction.Demand, ReflectionEmit = true)]
            get
            {
                if (_IsEqualToStrategy == null)
                {
                    lock (_SyncLock)
                    {
                        if (_IsEqualToStrategy == null)
                            _IsEqualToStrategy = BuildIsEqualToStrategy();
                    }
                }

                return _IsEqualToStrategy;
            }
        }
    }
}
