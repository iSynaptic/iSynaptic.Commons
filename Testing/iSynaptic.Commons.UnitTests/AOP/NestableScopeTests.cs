using System;
using System.Collections.Generic;
using System.Text;
using MbUnit.Framework;

namespace iSynaptic.Commons.UnitTests.AOP
{
    [TestFixture]
    public class NestableScopeTests
    {
        [Test]
        [ExpectedException(typeof(ApplicationException))]
        public void NestingAppDomainWithinThread()
        {
            using (new StubNestableScope(ScopeBounds.Thread))
            using (new StubNestableScope(ScopeBounds.AppDomain))
            {

            }
        }

        [Test]
        public void CurrentRetrievesScope()
        {
            using (StubNestableScope scope = new StubNestableScope())
            {
                Assert.AreEqual(scope, StubNestableScope.Current);
            }
        }

        [Test]
        public void CurrentRetreivesNestedScope()
        {
            using (StubNestableScope scope = new StubNestableScope())
            using (StubNestableScope scope2 = new StubNestableScope())
            {
                Assert.AreEqual(scope2, StubNestableScope.Current);
            }
        }

        [Test]
        public void ScopeAvailableInOtherMethods()
        {
            using (StubNestableScope scope = new StubNestableScope())
            {
                TestCurrentScope();
            }
        }

        private void TestCurrentScope()
        {
            Assert.IsNotNull(StubNestableScope.Current);
        }
    }
}
