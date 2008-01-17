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
    }
}
