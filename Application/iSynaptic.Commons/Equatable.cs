using System;

namespace iSynaptic.Commons
{
    //public static class Equatable
    //{
    //    public static bool IsEqualsTo<T>(this T @this, T other)
    //    {
    //        return Equatable<T>.IsEqualsTo(this, other);
    //    }

    //    public static int ToHashCode<T>(this T this)
    //    {
    //        return Equatable<T>.ToHashCode(this);
    //    }
    //}

    //public static class Equatable<T>
    //{
    //    //private static readonly object SyncLock = null;

    //    //private static Type _TargetType = null;
    //    //private static Func<T, T, bool> _IsEqualToStrategy = null;
    //    //private static Func<T, int> _ToHashCodeStrategy = null;

    //    //static Equatable()
    //    //{
    //    //    _TargetType = typeof (T);
    //    //    SyncLock = new object();
    //    //}

    //    public static bool IsEqualsTo(T source, T other)
    //    {
    //        if(ReferenceEquals(source, null) && ReferenceEquals(other, null))
    //            return true;

    //        if(ReferenceEquals(source, null))
    //            return false;

    //        if(ReferenceEquals(other, null))
    //            return false;

    //        if(source.GetType() != other.GetType())
    //            return false;

    //        return false;

    //        //return IsEqualToStrategy(source, other);
    //    }

    //    public static int ToHashCode(T source)
    //    {
    //        if (ReferenceEquals(source, null))
    //            return 0;

    //        throw new NotImplementedException();
    //    }

    //    //private static Func<T, T, bool> BuildIsEqualToStrategy()
    //    //{
    //    //    string dynamicMethodName = string.Format("Equatable<{0}>_IsEqualToDynamicStrategy", _TargetType.Name);
    //    //    DynamicMethod dynamicStrategyMethod = new DynamicMethod(dynamicMethodName,
    //    //                                                            _TargetType,
    //    //                                                            new[]
    //    //                                                                    {
    //    //                                                                        _TargetType,
    //    //                                                                        _TargetType,
    //    //                                                                    },
    //    //                                                            _TargetType, true);


    //    //    ILGenerator gen = dynamicStrategyMethod.GetILGenerator();

    //    //    return dynamicStrategyMethod.ToFunc<T, T, bool>();
    //    //}

    //    //private static Func<T, T, bool> IsEqualToStrategy
    //    //{
    //    //    [ReflectionPermission(SecurityAction.Demand, ReflectionEmit = true)]
    //    //    get
    //    //    {
    //    //        if (_IsEqualToStrategy == null)
    //    //        {
    //    //            lock (_SyncLock)
    //    //            {
    //    //                if (_IsEqualToStrategy == null)
    //    //                    _IsEqualToStrategy = BuildIsEqualToStrategy();
    //    //            }
    //    //        }

    //    //        return _IsEqualToStrategy;
    //    //    }
    //    //}
    //}
}
