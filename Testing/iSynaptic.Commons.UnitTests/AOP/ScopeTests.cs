using System;
using System.Collections.Generic;
using System.Text;

using MbUnit.Framework;

namespace iSynaptic.Commons.UnitTests.AOP
{
    [TestFixture]
    public class ScopeTests
    {
        [Test]
        [ExpectedException(typeof(ApplicationException))]
        public void NestedScopesNotAllowed()
        {
            using (new StubScope())
            using (new StubScope())
            {
            }
        }

        [Test]
        public void CurrentRetreivesScope()
        {
            using (StubScope scope = new StubScope())
            {
                Assert.AreEqual(scope, StubScope.Current);
            }
        }
    }
}
