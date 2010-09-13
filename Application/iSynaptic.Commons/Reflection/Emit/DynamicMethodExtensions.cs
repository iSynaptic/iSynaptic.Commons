using System;
using System.Reflection.Emit;

namespace iSynaptic.Commons.Reflection.Emit
{
    public static class DynamicMethodExtensions
    {
        public static Func<TRet> ToFunc<TRet>(this DynamicMethod self)
        {
            return (Func<TRet>)self.CreateDelegate(typeof(Func<TRet>));
        }

        public static Func<T1, TRet> ToFunc<T1, TRet>(this DynamicMethod self)
        {
            return (Func<T1, TRet>)self.CreateDelegate(typeof(Func<T1, TRet>));
        }

        public static Func<T1, T2, TRet> ToFunc<T1, T2, TRet>(this DynamicMethod self)
        {
            return (Func<T1, T2, TRet>)self.CreateDelegate(typeof(Func<T1, T2, TRet>));
        }

        public static Func<T1, T2, T3, TRet> ToFunc<T1, T2, T3, TRet>(this DynamicMethod self)
        {
            return (Func<T1, T2, T3, TRet>)self.CreateDelegate(typeof(Func<T1, T2, T3, TRet>));
        }

        public static Func<T1, T2, T3, T4, TRet> ToFunc<T1, T2, T3, T4, TRet>(this DynamicMethod self)
        {
            return (Func<T1, T2, T3, T4, TRet>)self.CreateDelegate(typeof(Func<T1, T2, T3, T4, TRet>));
        }
    }

}
