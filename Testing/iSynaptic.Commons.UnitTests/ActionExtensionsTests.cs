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
    }
}
