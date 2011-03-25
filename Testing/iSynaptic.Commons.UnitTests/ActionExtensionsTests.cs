using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace iSynaptic.Commons
{
    [TestFixture]
    public partial class ActionExtensionsTests
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
        public void CatchExceptions_WithNullAction()
        {
            Action action = null;
            Assert.Throws<ArgumentNullException>(() => action.CatchExceptions());
        }

        [Test]
        public void CatchExceptions_WithCollectionAndNullAction()
        {
            var exceptions = new List<Exception>();

            Action action = null;
            Assert.Throws<ArgumentNullException>(() => action.CatchExceptions(exceptions));
        }

        [Test]
        public void CatchExceptions()
        {
            Action action = () => { throw new InvalidOperationException(); };
            action = action.CatchExceptions();

            action();
        }

        [Test]
        public void CatchExceptionsWithCollection()
        {
            List<Exception> exceptions = new List<Exception>();

            Action action = () => { throw new InvalidOperationException(); };
            action = action.CatchExceptions(exceptions);

            action();

            Assert.AreEqual(1, exceptions.Count);
            Assert.IsTrue(exceptions[0].GetType() == typeof(InvalidOperationException));
        }

        [Test]
        public void MakeIdempotent_EnsuresActionsExecuteOnlyOnce()
        {
            int count = 0;
            Action action = () => count++;
            action = action.MakeIdempotent();

            action();
            action();
            action();

            Assert.AreEqual(1, count);
        }

        [Test]
        public void MakeIdempotent_WithNullArgument_ThrowsException()
        {
            Action action = null;
            Assert.Throws<ArgumentNullException>(() => action.MakeIdempotent());
        }

        [Test]
        public void FollowedBy_WithNullArgument_ReturnsOriginal()
        {
            bool executed = false;
            Action originalAction = () => executed = true;
            Action action = originalAction.FollowedBy(null);

            action();

            Assert.IsTrue(ReferenceEquals(originalAction, action));
            Assert.IsTrue(executed);
        }

        [Test]
        public void FollowedBy_ExtendingNullAction_ReturnsOriginal()
        {
            bool executed = false;
            Action originalAction = () => executed = true;
            Action action = ((Action)null).FollowedBy(originalAction);

            action();

            Assert.IsTrue(ReferenceEquals(originalAction, action));
            Assert.IsTrue(executed);
        }

        [Test]
        public void FollowedBy_CallsBothActionsInSuccession()
        {
            bool leftExecuted = false;
            bool rightExecuted = false;

            Action left = () => leftExecuted = true;
            Action right = () => rightExecuted = true;

            var action = left.FollowedBy(right);

            action();
            Assert.IsTrue(leftExecuted && rightExecuted);
        }
    }
}
