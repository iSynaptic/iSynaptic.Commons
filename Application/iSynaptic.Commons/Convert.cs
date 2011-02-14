using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using iSynaptic.Commons.Reflection;

namespace iSynaptic.Commons
{
    public static class Convert<TSource, TDest>
    {
        private static readonly Lazy<Func<TSource, TDest>> _Default;
        private static Func<TSource, TDest> _Strategy = null;

        static Convert()
        {
            _Default = new Lazy<Func<TSource, TDest>>(GetDefault);
        }

        public static TDest From(TSource source)
        {
            var strategy = _Strategy ?? _Default.Value;
            return strategy(source);
        }

        public static void SetStrategy(Func<TSource, TDest> strategy)
        {
            _Strategy = strategy;
        }

        private static Func<TSource, TDest> GetDefault()
        {
            var sourceType = typeof(TSource);
            var destType = typeof(TDest);

            if (sourceType.IsEnum)
                sourceType = Enum.GetUnderlyingType(sourceType);

            if (destType.IsEnum)
                destType = Enum.GetUnderlyingType(destType);

            if (destType.IsAssignableFrom(sourceType))
                return Cast<TSource, TDest>.With;

            var conversionMethod = destType.FindConversionMethod(sourceType, destType) ??
                                   sourceType.FindConversionMethod(sourceType, destType);

            if (conversionMethod != null)
                return (Func<TSource, TDest>)Delegate.CreateDelegate(typeof (Func<TSource, TDest>), conversionMethod);

            return source => (TDest)Convert.ChangeType(source, destType, Thread.CurrentThread.CurrentCulture);
        }
    }
}
