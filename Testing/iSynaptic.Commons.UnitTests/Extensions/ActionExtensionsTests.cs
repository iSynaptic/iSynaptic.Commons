using System;
using System.Collections.Generic;
using System.Text;
using MbUnit.Framework;

using iSynaptic.Commons.Extensions;

namespace iSynaptic.Commons.UnitTests.Extensions
{
    [TestFixture]
    public class ActionExtensionsTests : BaseTestFixture
    {
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
            AssertThrows<ArgumentNullException>(() => { action = action.MakeConditional(i => i >= 5); });
        }

        [Test]
        public void MakeConditionalWithNullCondition()
        {
            Action<int> action = i => { };
            AssertThrows<ArgumentNullException>(() => { action = action.MakeConditional(null); });
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
    }
}
