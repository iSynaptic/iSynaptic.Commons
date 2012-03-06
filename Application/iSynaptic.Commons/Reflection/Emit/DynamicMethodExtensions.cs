// The MIT License
// 
// Copyright (c) 2012 Jordan E. Terrell
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
using System.Reflection.Emit;

namespace iSynaptic.Commons.Reflection.Emit
{
    public static class DynamicMethodExtensions
    {
        public static Func<TRet> ToFunc<TRet>(this DynamicMethod @this)
        {
            Guard.NotNull(@this, "@this");
            return (Func<TRet>)@this.CreateDelegate(typeof(Func<TRet>));
        }

        public static Func<T1, TRet> ToFunc<T1, TRet>(this DynamicMethod @this)
        {
            Guard.NotNull(@this, "@this");
            return (Func<T1, TRet>)@this.CreateDelegate(typeof(Func<T1, TRet>));
        }

        public static Func<T1, T2, TRet> ToFunc<T1, T2, TRet>(this DynamicMethod @this)
        {
            Guard.NotNull(@this, "@this");
            return (Func<T1, T2, TRet>)@this.CreateDelegate(typeof(Func<T1, T2, TRet>));
        }

        public static Func<T1, T2, T3, TRet> ToFunc<T1, T2, T3, TRet>(this DynamicMethod @this)
        {
            Guard.NotNull(@this, "@this");
            return (Func<T1, T2, T3, TRet>)@this.CreateDelegate(typeof(Func<T1, T2, T3, TRet>));
        }

        public static Func<T1, T2, T3, T4, TRet> ToFunc<T1, T2, T3, T4, TRet>(this DynamicMethod @this)
        {
            Guard.NotNull(@this, "@this");
            return (Func<T1, T2, T3, T4, TRet>)@this.CreateDelegate(typeof(Func<T1, T2, T3, T4, TRet>));
        }
    }

}
