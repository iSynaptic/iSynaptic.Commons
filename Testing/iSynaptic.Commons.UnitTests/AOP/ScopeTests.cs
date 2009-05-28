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
        public void NestedAppDomainBoundScopesNotAllowed()
        {
            Assert.Throws<ApplicationException>(() =>
            {
                using (new StubScope(ScopeBounds.AppDomain))
                using (new StubScope(ScopeBounds.AppDomain))
                {
                }
            });
        }

        [Test]
        public void NestedThreadBoundScopesNotAllowed()
        {
            Assert.Throws<ApplicationException>(() =>
            {
                using (new StubScope(ScopeBounds.Thread))
                using (new StubScope(ScopeBounds.Thread))
                {
                }
            });
        }

        [Test]
        public void NestedThreadBoundInAppDomainBoundScopeNotAllowed()
        {
            Assert.Throws<ApplicationException>(() =>
            {
                using (new StubScope(ScopeBounds.AppDomain))
                using (new StubScope(ScopeBounds.Thread))
                {
                }
            });
        }

        [Test]
        public void NestedAppDomainBoundInThreadBoundScopeNotAllowed()
        {
            Assert.Throws<ApplicationException>(() =>
            {
                using (new StubScope(ScopeBounds.Thread))
                using (new StubScope(ScopeBounds.AppDomain))
                {
                }
            });
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
        public void UndefinedBoundsValues()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                using (StubScope scope = new StubScope((ScopeBounds)73))
                {
                }
            });
        }
    }
}
