using System;
using System.Collections.Generic;
using System.Text;

namespace iSynaptic.Commons.Extensions
{
    public static class FuncExtensions
    {
        public static Func<TRet> Curry<T1, TRet>(this Func<T1, TRet> f, T1 arg1)
        {
            return () => f(arg1);
        }

        public static Func<TRet> Curry<T1, T2, TRet>(this Func<T1, T2, TRet> f, T1 arg1, T2 arg2)
        {
            return () => f(arg1, arg2);
        }

        public static Func<T2, TRet> Curry<T1, T2, TRet>(this Func<T1, T2, TRet> f, T1 arg1)
        {
            return t2 => f(arg1, t2);
        }

        public static Func<TRet> Curry<T1, T2, T3, TRet>(this Func<T1, T2, T3, TRet> f, T1 arg1, T2 arg2, T3 arg3)
        {
            return () => f(arg1, arg2, arg3);
        }

        public static Func<T3, TRet> Curry<T1, T2, T3, TRet>(this Func<T1, T2, T3, TRet> f, T1 arg1, T2 arg2)
        {
            return t3 => f(arg1, arg2, t3);
        }

        public static Func<T2, T3, TRet> Curry<T1, T2, T3, TRet>(this Func<T1, T2, T3, TRet> f, T1 arg1)
        {
            return (t2, t3) => f(arg1, t2, t3);
        }

        public static Func<TRet> Curry<T1, T2, T3, T4, TRet>(this Func<T1, T2, T3, T4, TRet> f, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            return () => f(arg1, arg2, arg3, arg4);
        }

        public static Func<T4, TRet> Curry<T1, T2, T3, T4, TRet>(this Func<T1, T2, T3, T4, TRet> f, T1 arg1, T2 arg2, T3 arg3)
        {
            return t4 => f(arg1, arg2, arg3, t4);
        }

        public static Func<T3, T4, TRet> Curry<T1, T2, T3, T4, TRet>(this Func<T1, T2, T3, T4, TRet> f, T1 arg1, T2 arg2)
        {
            return (t3, t4) => f(arg1, arg2, t3, t4);
        }

        public static Func<T2, T3, T4, TRet> Curry<T1, T2, T3, T4, TRet>(this Func<T1, T2, T3, T4, TRet> f, T1 arg1)
        {
            return (t2, t3, t4) => f(arg1, t2, t3, t4);
        }
    }
}
