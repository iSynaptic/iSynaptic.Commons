using System;
using System.Collections.Generic;
using System.Linq;

namespace iSynaptic.Commons
{
    public static class EnumExtensions
    {
        public static bool IsDefined(this Enum input)
        {
            return Enum.IsDefined(input.GetType(), input);
        }

        public static bool Contains<T>(this Enum @this, T flag)
        {
            Type thisType = @this.GetType();
            Type flagType = typeof(T);

            if (flagType.IsEnum != true)
                throw new ArgumentException("Parameter must be an enumeration.", "flag");

            if (flagType != thisType)
                throw new ArgumentException(string.Format("Parameter must be of type '{0}'.", flagType.Name), "flag");

            if(Enum.IsDefined(flagType, flag) != true)
                throw new ArgumentException("Only defined values can be provided. Try using ContainsAny() or ContainsAll().", "flag");

            ulong thisValue = Convert.ToUInt64(@this);
            ulong flagValue = Convert.ToUInt64(flag);

            if ((thisValue & flagValue) == flagValue && thisValue != 0)
                return true;

            return false;
        }

        public static bool ContainsAny<T>(this Enum @this, params T[] flags)
        {
            Type thisType = @this.GetType();
            Type flagType = typeof(T);

            if (flagType.IsEnum != true)
                throw new ArgumentException("Parameter base type must be an enumeration.", "flags");

            if (flagType != thisType)
                throw new ArgumentException(string.Format("Parameter base type must be of type '{0}'.", flagType.Name), "flags");

            ulong thisValue = Convert.ToUInt64(@this);

            var values = flags
                .OfType<Enum>()
                .SelectMany(GetFlagsCore<T>)
                .Distinct();

            return values
                .Select(flag => Convert.ToUInt64(flag))
                .Any(flagValue => (thisValue & flagValue) == flagValue && thisValue != 0);
        }

        public static bool ContainsAll<T>(this Enum @this, params T[] flags)
        {
            Type thisType = @this.GetType();
            Type flagType = typeof(T);

            if (flagType.IsEnum != true)
                throw new ArgumentException("Parameter base type must be an enumeration.", "flags");

            if (flagType != thisType)
                throw new ArgumentException(string.Format("Parameter base type must be of type '{0}'.", flagType.Name), "flags");

            ulong thisValue = Convert.ToUInt64(@this);

            var values = flags
                .OfType<Enum>()
                .SelectMany(GetFlagsCore<T>)
                .Distinct();

            foreach (T flag in values)
            {
                ulong flagValue = Convert.ToUInt64(flag);

                if (((thisValue & flagValue) == flagValue && thisValue != 0) != true)
                    return false;
            }

            return true;
        }

        public static IEnumerable<T> GetFlags<T>(this Enum @this)
        {
            Type thisType = @this.GetType();
            Type expectedType = typeof(T);

            if (expectedType.IsEnum != true)
                throw new ArgumentException("Type parameter must be an enumeration.", "T");

            if (expectedType != thisType)
                throw new ArgumentException(string.Format("Type parameter must be of type '{0}'.", expectedType.Name), "T");

            return GetFlagsCore<T>(@this).OfType<T>();
        }

        private static IEnumerable<T> GetFlagsCore<T>(Enum @this)
        {
            Type thisType = typeof(T);

            ulong thisValue = Convert.ToUInt64(@this);
            var values = Enum.GetValues(thisType).OfType<T>();

            return from value in values
                   let i64 = Convert.ToUInt64(value)
                   where (thisValue & i64) == i64 && thisValue != 0
                   select value;
        }
    }
}
