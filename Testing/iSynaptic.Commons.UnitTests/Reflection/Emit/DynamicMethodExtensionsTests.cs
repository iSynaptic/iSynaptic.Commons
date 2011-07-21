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
using System.Reflection.Emit;
using NUnit.Framework;

namespace iSynaptic.Commons.Reflection.Emit
{
    [TestFixture]
    public class DynamicMethodExtensionsTests
    {
        [Test]
        public void ToFuncWithOneTypeParameter()
        {
            DynamicMethod method = new DynamicMethod(Guid.NewGuid().ToString(), typeof(int), null);
            var il = method.GetILGenerator();

            il.Emit(OpCodes.Ldc_I4_1);
            il.Emit(OpCodes.Ret);

            var func = DynamicMethodExtensions.ToFunc<int>(method);

            Assert.AreEqual(1, func());
        }

        [Test]
        public void ToFuncWithTwoTypeParameter()
        {
            DynamicMethod method = new DynamicMethod(Guid.NewGuid().ToString(), typeof(int), new Type[] { typeof(int) });
            var il = method.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ret);

            var func = DynamicMethodExtensions.ToFunc<int, int>(method);

            Assert.AreEqual(5, func(5));
        }

        [Test]
        public void ToFuncWithThreeTypeParameter()
        {
            DynamicMethod method = new DynamicMethod(Guid.NewGuid().ToString(), typeof(int), new Type[] { typeof(int), typeof(int) });
            var il = method.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Add);
            il.Emit(OpCodes.Ret);

            var func = DynamicMethodExtensions.ToFunc<int, int, int>(method);

            Assert.AreEqual(11, func(5, 6));
        }

        [Test]
        public void ToFuncWithFourTypeParameter()
        {
            DynamicMethod method = new DynamicMethod(Guid.NewGuid().ToString(), typeof(int), new Type[] { typeof(int), typeof(int), typeof(int) });
            var il = method.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Add);
            il.Emit(OpCodes.Ldarg_2);
            il.Emit(OpCodes.Add);

            il.Emit(OpCodes.Ret);

            var func = DynamicMethodExtensions.ToFunc<int, int, int, int>(method);

            Assert.AreEqual(18, func(5, 6, 7));
        }

        [Test]
        public void ToFuncWithFiveTypeParameter()
        {
            DynamicMethod method = new DynamicMethod(Guid.NewGuid().ToString(), typeof(int), new Type[] { typeof(int), typeof(int), typeof(int), typeof(int) });
            var il = method.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Add);
            il.Emit(OpCodes.Ldarg_2);
            il.Emit(OpCodes.Add);
            il.Emit(OpCodes.Ldarg_3);
            il.Emit(OpCodes.Add);

            il.Emit(OpCodes.Ret);

            var func = DynamicMethodExtensions.ToFunc<int, int, int, int, int>(method);

            Assert.AreEqual(26, func(5, 6, 7, 8));
        }
    }
}
