using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace iSynaptic.Commons.Extensions.ObjectExtensions
{
    public static class MethodReflectionObjectExtensions
    {
        private static T GetDelegate<T>(string methodName, object target, params Type[] types)
        {
            Type targetType = target.GetType();

            MethodInfo info = targetType.GetMethod(methodName,
               BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.FlattenHierarchy,
               null,
               types ?? Type.EmptyTypes,
               null);

            if (info != null)
            {
                if(info.IsStatic)
                    return (T)(object)Delegate.CreateDelegate(typeof(T), info);
                else
                    return (T)(object)Delegate.CreateDelegate(typeof(T), target, info);
            }
            else
                return default(T);
        }

        public static Func<TResult> GetFunc<TResult>(this object target, string methodName)
        {
            return GetDelegate<Func<TResult>>(methodName, target, null);
        }

        public static Func<T1, TResult> GetFunc<T1, TResult>(this object target, string methodName)
        {
            return GetDelegate<Func<T1, TResult>>(methodName, target, typeof(T1));
        }

        public static Func<T1, T2, TResult> GetFunc<T1, T2, TResult>(this object target, string methodName)
        {
            return GetDelegate<Func<T1, T2, TResult>>(methodName, target, typeof(T1), typeof(T2));
        }

        public static Func<T1, T2, T3, TResult> GetFunc<T1, T2, T3, TResult>(this object target, string methodName)
        {
            return GetDelegate<Func<T1, T2, T3, TResult>>(methodName, target, typeof(T1), typeof(T2), typeof(T3));
        }

        public static Func<T1, T2, T3, T4, TResult> GetFunc<T1, T2, T3, T4, TResult>(this object target, string methodName)
        {
            return GetDelegate<Func<T1, T2, T3, T4, TResult>>(methodName, target, typeof(T1), typeof(T2), typeof(T3), typeof(T4));
        }

        public static Action GetAction(this object target, string methodName)
        {
            return GetDelegate<Action>(methodName, target, null);
        }

        public static Action<T1> GetAction<T1>(this object target, string methodName)
        {
            return GetDelegate<Action<T1>>(methodName, target, typeof(T1));
        }

        public static Action<T1, T2> GetAction<T1, T2>(this object target, string methodName)
        {
            return GetDelegate<Action<T1, T2>>(methodName, target, typeof(T1), typeof(T2));
        }

        public static Action<T1, T2, T3> GetAction<T1, T2, T3>(this object target, string methodName)
        {
            return GetDelegate<Action<T1, T2, T3>>(methodName, target, typeof(T1), typeof(T2), typeof(T3));
        }

        public static Action<T1, T2, T3, T4> GetAction<T1, T2, T3, T4>(this object target, string methodName)
        {
            return GetDelegate<Action<T1, T2, T3, T4>>(methodName, target, typeof(T1), typeof(T2), typeof(T3), typeof(T4));
        }
    }
}
