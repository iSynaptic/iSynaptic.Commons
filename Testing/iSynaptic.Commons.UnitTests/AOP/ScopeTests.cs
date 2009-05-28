using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;

using iSynaptic.Commons.AOP;

namespace iSynaptic.Commons.UnitTests.AOP
{
    [TestFixture]
    public class ScopeTests
    {
        [Test]
        [ExpectedException(typeof(ApplicationException))]
        public void NestedAppDomainBoundScopesNotAllowed()
        {
            using (new StubScope(ScopeBounds.AppDomain))
            using (new StubScope(ScopeBounds.AppDomain))
            {
            }
        }

        [Test]
        [ExpectedException(typeof(ApplicationException))]
        public void NestedThreadBoundScopesNotAllowed()
        {
            using (new StubScope(ScopeBounds.Thread))
            using (new StubScope(ScopeBounds.Thread))
            {
            }
        }

        [Test]
        [ExpectedException(typeof(ApplicationException))]
        public void NestedThreadBoundInAppDomainBoundScopeNotAllowed()
        {
            using (new StubScope(ScopeBounds.AppDomain))
            using (new StubScope(ScopeBounds.Thread))
            {
            }
        }

        [Test]
        [ExpectedException(typeof(ApplicationException))]
        public void NestedAppDomainBoundInThreadBoundScopeNotAllowed()
        {
            using (new StubScope(ScopeBounds.Thread))
            using (new StubScope(ScopeBounds.AppDomain))
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

        [Test]
        public void ThreadBoundScopeNotAvailableOnAnotherThread()
        {
            bool isAvailable = true;

            Action assertCurrentScopeIsNull = delegate()
            {
                isAvailable = StubScope.Current != null;
            };

            using (StubScope scope = new StubScope(ScopeBounds.Thread))
            {
                IAsyncResult result = assertCurrentScopeIsNull.BeginInvoke(null, null);

                result.AsyncWaitHandle.WaitOne();
                assertCurrentScopeIsNull.EndInvoke(result);
            }

            Assert.IsFalse(isAvailable);
        }

        [Test]
        public void AppDomainBoundScopeIsAvailableOnAnotherThread()
        {
            bool isAvailable = false;

            Action assertCurrentScopeIsNull = delegate()
            {
                isAvailable = StubScope.Current != null;
            };

            using (StubScope scope = new StubScope(ScopeBounds.AppDomain))
            {
                IAsyncResult result = assertCurrentScopeIsNull.BeginInvoke(null, null);

                result.AsyncWaitHandle.WaitOne();
                assertCurrentScopeIsNull.EndInvoke(result);
            }

            Assert.IsTrue(isAvailable);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void UndefinedBoundsValues()
        {
            using (StubScope scope = new StubScope((ScopeBounds)73))
            {
            }
        }
    }
}
