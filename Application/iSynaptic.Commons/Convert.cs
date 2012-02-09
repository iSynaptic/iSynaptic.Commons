// The MIT License
// 
// Copyright (c) 2011 Jordan E. Terrell
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

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
        private static readonly Lazy<Func<TSource, TDest>> Default;
        private static Func<TSource, TDest> _Strategy = null;

        static Convert()
        {
            Default = new Lazy<Func<TSource, TDest>>(GetDefault);
        }

        public static TDest From(TSource source)
        {
            var strategy = _Strategy ?? Default.Value;
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
