using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

using iSynaptic.Commons.Extensions;
using iSynaptic.Commons.Testing.NUnit;

namespace iSynaptic.Commons.Extensions
{
    [TestFixture]
    public class ActionExtensionsTests : NUnitBaseTestFixture
    {
        [Test]
        public void CurryOneOfOneArgument()
        {
            int out1 = 0;
            Action<int> action = (a1) => out1 = a1;
            Action curried = action.Curry(6);

            curried();

            Assert.AreEqual(6, out1);
        }

        [Test]
        public void CurryOneOfTwoArguments()
        {
            int out1 = 0;
            int out2 = 0;

            Action<int, int> action = (a1, a2) => { out1 = a1; out2 = a2; };
            Action<int> curried = action.Curry(6);

            curried(7);

            Assert.AreEqual(6, out1);
            Assert.AreEqual(7, out2);
        }

        [Test]
        public void CurryTwoOfTwoArguments()
        {
            int out1 = 0;
            int out2 = 0;

            Action<int, int> action = (a1, a2) => { out1 = a1; out2 = a2; };
            Action curried = action.Curry(6, 7);

            curried();

            Assert.AreEqual(6, out1);
            Assert.AreEqual(7, out2);
        }

        [Test]
        public void CurryOneOfThreeArguments()
        {
            int out1 = 0;
            int out2 = 0;
            int out3 = 0;

            Action<int, int, int> action = (a1, a2, a3) =>
            {
                out1 = a1;
                out2 = a2;
                out3 = a3;
            };

            Action<int, int> curried = action.Curry(6);

            curried(7, 8);

            Assert.AreEqual(6, out1);
            Assert.AreEqual(7, out2);
            Assert.AreEqual(8, out3);
        }

        [Test]
        public void CurryTwoOfThreeArguments()
        {
            int out1 = 0;
            int out2 = 0;
            int out3 = 0;

            Action<int, int, int> action = (a1, a2, a3) =>
            {
                out1 = a1;
                out2 = a2;
                out3 = a3;
            };

            Action<int> curried = action.Curry(6, 7);

            curried(8);

            Assert.AreEqual(6, out1);
            Assert.AreEqual(7, out2);
            Assert.AreEqual(8, out3);
        }

        [Test]
        public void CurryThreeOfThreeArguments()
        {
            int out1 = 0;
            int out2 = 0;
            int out3 = 0;

            Action<int, int, int> action = (a1, a2, a3) =>
            {
                out1 = a1;
                out2 = a2;
                out3 = a3;
            };

            Action curried = action.Curry(6, 7, 8);
            curried();

            Assert.AreEqual(6, out1);
            Assert.AreEqual(7, out2);
            Assert.AreEqual(8, out3);
        }

        [Test]
        public void CurryOneOfFourArguments()
        {
            int out1 = 0;
            int out2 = 0;
            int out3 = 0;
            int out4 = 0;

            Action<int, int, int, int> action = (a1, a2, a3, a4) =>
            {
                out1 = a1;
                out2 = a2;
                out3 = a3;
                out4 = a4;
            };

            Action<int, int, int> curried = action.Curry(6);

            curried(7, 8, 9);

            Assert.AreEqual(6, out1);
            Assert.AreEqual(7, out2);
            Assert.AreEqual(8, out3);
            Assert.AreEqual(9, out4);
        }

        [Test]
        public void CurryTwoOfFourArguments()
        {
            int out1 = 0;
            int out2 = 0;
            int out3 = 0;
            int out4 = 0;

            Action<int, int, int, int> action = (a1, a2, a3, a4) =>
            {
                out1 = a1;
                out2 = a2;
                out3 = a3;
                out4 = a4;
            };

            Action<int, int> curried = action.Curry(6, 7);

            curried(8, 9);

            Assert.AreEqual(6, out1);
            Assert.AreEqual(7, out2);
            Assert.AreEqual(8, out3);
            Assert.AreEqual(9, out4);
        }

        [Test]
        public void CurryThreeOfFourArguments()
        {
            int out1 = 0;
            int out2 = 0;
            int out3 = 0;
            int out4 = 0;

            Action<int, int, int, int> action = (a1, a2, a3, a4) =>
            {
                out1 = a1;
                out2 = a2;
                out3 = a3;
                out4 = a4;
            };

            Action<int> curried = action.Curry(6, 7, 8);
            curried(9);

            Assert.AreEqual(6, out1);
            Assert.AreEqual(7, out2);
            Assert.AreEqual(8, out3);
            Assert.AreEqual(9, out4);
        }

        [Test]
        public void CurryFourOfFourArguments()
        {
            int out1 = 0;
            int out2 = 0;
            int out3 = 0;
            int out4 = 0;

            Action<int, int, int, int> action = (a1, a2, a3, a4) =>
            {
                out1 = a1;
                out2 = a2;
                out3 = a3;
                out4 = a4;
            };

            Action curried = action.Curry(6, 7, 8, 9);
            curried();

            Assert.AreEqual(6, out1);
            Assert.AreEqual(7, out2);
            Assert.AreEqual(8, out3);
            Assert.AreEqual(9, out4);
        }

        [Test]
        public void ToDisposableWithNull()
        {
            Action action = null;
            Assert.Throws<ArgumentNullException>(() => action.ToDisposable());
        }

        [Test]
        public void ToDisposable()
        {
            bool wasDisposed = false;
            Action action = () => wasDisposed = true;

            using (action.ToDisposable())
            {
                Assert.IsFalse(wasDisposed);
            }

            Assert.IsTrue(wasDisposed);
        }

        [Test]
        public void MakeConditionalWithNull()
        {
            Action<int> action = null;
            Assert.Throws<ArgumentNullException>(() => { action = action.MakeConditional(i => i >= 5); });
        }

        [Test]
        public void MakeConditionalWithNullCondition()
        {
            Action<int> action = i => { };
            Assert.Throws<ArgumentNullException>(() => { action = action.MakeConditional(null); });
        }

        [Test]
        public void MakeConditional()
        {
            bool actionExecuted = false;
            Action<int> action = i => actionExecuted = true;
            action = action.MakeConditional(i => i >= 5);

            action(1);
            Assert.IsFalse(actionExecuted);

            action(4);
            Assert.IsFalse(actionExecuted);

            action(5);
            Assert.IsTrue(actionExecuted);
        }

        [Test]
        public void MakeConditionalWithFalseAction()
        {
            int falseCount = 0;
            bool actionExecuted = false;

            Action<int> action = i => actionExecuted = true;
            action = action.MakeConditional(i => falseCount++, i => i >= 5);

            action(1);
            Assert.IsFalse(actionExecuted);
            Assert.AreEqual(1, falseCount);

            action(4);
            Assert.IsFalse(actionExecuted);
            Assert.AreEqual(2, falseCount);

            action(5);
            Assert.IsTrue(actionExecuted);
            Assert.AreEqual(2, falseCount);
        }

        [Test]
        public void CatchExceptionsWithNullAction()
        {
            Action<int> action = null;
            Assert.Throws<ArgumentNullException>(() => action.CatchExceptions());
        }

        [Test]
        public void CatchExceptions()
        {
            Action<int> action = (i) => { throw new ArgumentOutOfRangeException(); };
            action = action.CatchExceptions();

            action(1);
        }

        [Test]
        public void CatchExceptionsWithCollection()
        {
            List<Exception> exceptions = new List<Exception>();

            Action<int> action = (i) => { throw new ArgumentOutOfRangeException(); };
            action = action.CatchExceptions(exceptions);

            action(1);
            
            Assert.AreEqual(1, exceptions.Count);
            Assert.IsTrue(exceptions[0].GetType() == typeof(ArgumentOutOfRangeException));
        }
    }
}
