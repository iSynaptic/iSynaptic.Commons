using System;
using System.Collections.Generic;

namespace iSynaptic.Commons
{
    public static partial class FuncExtensions
    {
        public static Action ToAction<TRet>(this Func<TRet> self)
        {
            Guard.NotNull(self, "self");
            return () => self();
        }

        public static Func<T, bool> And<T>(this Func<T, bool> self, Func<T, bool> right)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(right, "right");

            return input => self(input) && right(input);
        }

        public static Func<T, bool> Or<T>(this Func<T, bool> self, Func<T, bool> right)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(right, "right");
            
            return input => self(input) || right(input);
        }

        public static Func<T, bool> XOr<T>(this Func<T, bool> self, Func<T, bool> right)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(right, "right");

            return input => self(input) ^ right(input);
        }
    }
}
