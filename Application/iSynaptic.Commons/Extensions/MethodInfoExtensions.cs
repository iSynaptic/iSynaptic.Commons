using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace iSynaptic.Commons.Extensions
{
    public static class MethodInfoExtensions
    {
        public static Func<TRet> ToFunc<TRet>(this MethodInfo self)
        {
            return (Func<TRet>)Delegate.CreateDelegate(typeof(Func<TRet>), self);
        }

        public static Func<T1, TRet> ToFunc<T1, TRet>(this MethodInfo self)
        {
            return (Func<T1, TRet>)Delegate.CreateDelegate(typeof(Func<T1, TRet>), self);
        }

        public static Func<T1, T2, TRet> ToFunc<T1, T2, TRet>(this MethodInfo self)
        {
            return (Func<T1, T2, TRet>)Delegate.CreateDelegate(typeof(Func<T1, T2, TRet>), self);
        }

        public static Func<T1, T2, T3, TRet> ToFunc<T1, T2, T3, TRet>(this MethodInfo self)
        {
            return (Func<T1, T2, T3, TRet>)Delegate.CreateDelegate(typeof(Func<T1, T2, T3, TRet>), self);
        }

        public static Func<T1, T2, T3, T4, TRet> ToFunc<T1, T2, T3, T4, TRet>(this MethodInfo self)
        {
            return (Func<T1, T2, T3, T4, TRet>)Delegate.CreateDelegate(typeof(Func<T1, T2, T3, T4, TRet>), self);
        }
    }
}
