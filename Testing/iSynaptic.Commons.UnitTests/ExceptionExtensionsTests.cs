using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace iSynaptic.Commons
{
    [TestFixture]
    public class ExceptionExtensionsTests
    {
        [Test]
        public void ThrowAsInnerExceptionIfNeeded_SetsInnerException()
        {
            var originalException = new InvalidOperationException("Hello, World!");
            try
            {
                try
                {
                    throw originalException;
                }
                catch (Exception ex)
                {
                    ex.ThrowAsInnerExceptionIfNeeded();
                }
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOf<InvalidOperationException>(ex);
                Assert.IsNotNull(ex.InnerException);
                Assert.IsTrue(ReferenceEquals(originalException, ex.InnerException));
            }
        }

        [Test]
        public void ThrowAsInnerExceptionIfNeeded_ThrowsPreviouslyUnthrownException()
        {
            var exception = Assert.Throws<InvalidOperationException>(
                () => new InvalidOperationException().ThrowAsInnerExceptionIfNeeded());

            Assert.IsNull(exception.InnerException);
        }
    }
}
