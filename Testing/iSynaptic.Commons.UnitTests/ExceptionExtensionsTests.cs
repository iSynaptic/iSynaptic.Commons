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
        public void ThrowPreservingCallStack_PreservesOriginalCallStack()
        {
            try
            {
                try
                {
                    throw new InvalidOperationException();
                }
                catch (Exception ex)
                {
                    ex.ThrowPreservingCallStack();
                }
            }
            catch (Exception ex)
            {
                Assert.IsFalse(ex.StackTrace.Substring(0, ex.StackTrace.IndexOf(Environment.NewLine)).Contains("ExceptionExtensions.ThrowPreservingCallStack"));   
            }
        }

        [Test]
        public void ThrowPreserviceCallStack_ThrowsPreviouslyUnthrownException()
        {
            Assert.Throws<InvalidOperationException>(() => new InvalidOperationException().ThrowPreservingCallStack());
        }
    }
}
