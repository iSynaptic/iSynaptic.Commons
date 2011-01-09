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
        public void Rethrow_UsesOriginalCallStack()
        {
            try
            {
                try
                {
                    throw new InvalidOperationException();
                }
                catch (Exception ex)
                {
                    ex.Rethrow();
                }
            }
            catch (Exception ex)
            {
                Assert.IsFalse(ex.StackTrace.Substring(0, ex.StackTrace.IndexOf(Environment.NewLine)).Contains("ExceptionExtensions.Rethrow"));   
            }
            
        }
    }
}
