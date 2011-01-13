using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace iSynaptic.Commons
{
    [TestFixture]
    public class ActionExtensionsTests
    {
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
            Action<int> actionT1 = null;
            Assert.Throws<ArgumentNullException>(() => actionT1.CatchExceptions());

            Action action = null;
            Assert.Throws<ArgumentNullException>(() => action.CatchExceptions());
        }

        [Test]
        public void CatchExceptionsWithCollectionAndNullAction()
        {
            var exceptions = new List<Exception>();

            Action<int> actionT1 = null;
            Assert.Throws<ArgumentNullException>(() => actionT1.CatchExceptions(exceptions));

            Action action = null;
            Assert.Throws<ArgumentNullException>(() => action.CatchExceptions(exceptions));
        }

        [Test]
        public void CatchExceptions()
        {
            Action<int> actionT1 = (i) => { throw new InvalidOperationException(); };
            actionT1 = actionT1.CatchExceptions();

            actionT1(1);

            Action action = () => { throw new InvalidOperationException(); };
            action = action.CatchExceptions();

            action();
        }

        [Test]
        public void CatchExceptionsWithCollection()
        {
            List<Exception> exceptions = new List<Exception>();

            Action<int> actionT1 = (i) => { throw new InvalidOperationException(); };
            actionT1 = actionT1.CatchExceptions(exceptions);

            actionT1(1);
            
            Assert.AreEqual(1, exceptions.Count);
            Assert.IsTrue(exceptions[0].GetType() == typeof(InvalidOperationException));

            exceptions.Clear();

            Action action = () => { throw new InvalidOperationException(); };
            action = action.CatchExceptions(exceptions);

            action();

            Assert.AreEqual(1, exceptions.Count);
            Assert.IsTrue(exceptions[0].GetType() == typeof(InvalidOperationException));
        }
    }
}
