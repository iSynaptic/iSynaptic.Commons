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
