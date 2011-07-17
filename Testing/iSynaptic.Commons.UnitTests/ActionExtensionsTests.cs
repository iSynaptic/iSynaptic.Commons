using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
