using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Runtime.CompilerServices;

namespace iSynaptic.Commons.Extensions
{
    public static class EnumExtensions
    {
        public static bool IsDefined(this Enum input)
        {
            return Enum.IsDefined(input.GetType(), input);
        }

        public static bool Contains<T>(this Enum self, T flag)
        {
            Type selfType = self.GetType();
            Type flagType = typeof(T);

            if (flagType.IsEnum != true)
                throw new ArgumentException("Parameter must be an enumeration.", "flag");

            if (flagType != selfType)
                throw new ArgumentException(string.Format("Parameter must be of type '{0}'.", flagType.Name), "flag");

            if(Enum.IsDefined(flagType, flag) != true)
                throw new ArgumentException("Only defined values can be provided. Try using ContainsAny() or ContainsAll().", "flag");

            ulong selfValue = Convert.ToUInt64(self);
            ulong flagValue = Convert.ToUInt64(flag);

            if ((selfValue & flagValue) == flagValue && selfValue != 0)
                return true;

            return false;
        }

        public static bool ContainsAny<T>(this Enum self, params T[] flags)
        {
            Type selfType = self.GetType();
            Type flagType = typeof(T);

            if (flagType.IsEnum != true)
                throw new ArgumentException("Parameter base type must be an enumeration.", "flags");

            if (flagType != selfType)
                throw new ArgumentException(string.Format("Parameter base type must be of type '{0}'.", flagType.Name), "flags");

            ulong selfValue = Convert.ToUInt64(self);
            var values = flags.OfType<Enum>().SelectMany<Enum, T>(source => GetFlagsCore<T>(source)).Distinct();

            foreach (T flag in values)
            {
                ulong flagValue = Convert.ToUInt64(flag);

                if ((selfValue & flagValue) == flagValue && selfValue != 0)
                    return true;
            }

            return false;
        }

        public static bool ContainsAll<T>(this Enum self, params T[] flags)
        {
            Type selfType = self.GetType();
            Type flagType = typeof(T);

            if (flagType.IsEnum != true)
                throw new ArgumentException("Parameter base type must be an enumeration.", "flags");

            if (flagType != selfType)
                throw new ArgumentException(string.Format("Parameter base type must be of type '{0}'.", flagType.Name), "flags");

            ulong selfValue = Convert.ToUInt64(self);
            var values = flags.OfType<Enum>().SelectMany<Enum, T>(source => GetFlagsCore<T>(source)).Distinct();

            foreach (T flag in values)
            {
                ulong flagValue = Convert.ToUInt64(flag);

                if (((selfValue & flagValue) == flagValue && selfValue != 0) != true)
                    return false;
            }

            return true;
        }

        public static IEnumerable<T> GetFlags<T>(this Enum self)
        {
            Type selfType = self.GetType();
            Type expectedType = typeof(T);

            if (expectedType.IsEnum != true)
                throw new ArgumentException("Type parameter must be an enumeration.", "T");

            if (expectedType != selfType)
                throw new ArgumentException(string.Format("Type parameter must be of type '{0}'.", expectedType.Name), "T");

            return GetFlagsCore<T>(self).OfType<T>();
        }

        private static IEnumerable<T> GetFlagsCore<T>(Enum self)
        {
            Type selfType = typeof(T);

            ulong selfValue = Convert.ToUInt64(self);
            var values = Enum.GetValues(selfType).OfType<T>();

            foreach (T flag in values)
            {
                ulong flagValue = Convert.ToUInt64(flag);

                if ((selfValue & flagValue) == flagValue && selfValue != 0)
                    yield return flag;
            }
        }
    }
}
