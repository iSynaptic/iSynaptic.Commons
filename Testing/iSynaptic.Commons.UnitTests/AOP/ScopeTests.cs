using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;

using iSynaptic.Commons.AOP;

namespace iSynaptic.Commons.AOP
{
    [TestFixture]
    public class ScopeTests
    {
        [Test]
        public void NestedAppDomainBoundScopesNotAllowed()
        {
            Assert.Throws<ApplicationException>(() =>
            {
                using (new StubScope(ScopeBounds.AppDomain, ScopeNesting.Prohibited))
                using (new StubScope(ScopeBounds.AppDomain, ScopeNesting.Prohibited))
                {
                }
            });
        }

        [Test]
        public void NestedThreadBoundScopesNotAllowed()
        {
            Assert.Throws<ApplicationException>(() =>
            {
                using (new StubScope(ScopeBounds.Thread, ScopeNesting.Prohibited))
                using (new StubScope(ScopeBounds.Thread, ScopeNesting.Prohibited))
                {
                }
            });
        }

        [Test]
        public void NestedThreadBoundInAppDomainBoundScopeNotAllowed()
        {
            Assert.Throws<ApplicationException>(() =>
            {
                using (new StubScope(ScopeBounds.AppDomain, ScopeNesting.Prohibited))
                using (new StubScope(ScopeBounds.Thread, ScopeNesting.Prohibited))
                {
                }
            });
        }

        [Test]
        public void NestedAppDomainBoundInThreadBoundScopeNotAllowed()
        {
            Assert.Throws<ApplicationException>(() =>
            {
                using (new StubScope(ScopeBounds.Thread, ScopeNesting.Prohibited))
                using (new StubScope(ScopeBounds.AppDomain, ScopeNesting.Prohibited))
                {
                }
            });
        }

        [Test]
        public void CurrentRetreivesScope()
        {
            using (StubScope scope = new StubScope(ScopeBounds.Thread, ScopeNesting.Prohibited))
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

            using (StubScope scope = new StubScope(ScopeBounds.Thread, ScopeNesting.Prohibited))
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

            using (StubScope scope = new StubScope(ScopeBounds.AppDomain, ScopeNesting.Prohibited))
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
                using (StubScope scope = new StubScope((ScopeBounds)73, ScopeNesting.Prohibited))
                {
                }
            });
        }

        [Test]
        public void UndefinedNestingValues()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                using (StubScope scope = new StubScope(ScopeBounds.Thread, (ScopeNesting)73))
                {
                }
            });
        }

        [Test]
        public void NestingAppDomainWithinThread()
        {
            Assert.Throws<ApplicationException>(() =>
            {
                using (new StubScope(ScopeBounds.Thread, ScopeNesting.Allowed))
                using (new StubScope(ScopeBounds.AppDomain, ScopeNesting.Allowed))
                {

                }
            });
        }

        [Test]
        public void CurrentRetrievesScope()
        {
            using (StubScope scope = new StubScope(ScopeBounds.Thread, ScopeNesting.Allowed))
            {
                Assert.AreEqual(scope, StubScope.Current);
            }
        }

        [Test]
        public void CurrentRetreivesNestedScope()
        {
            using (StubScope scope = new StubScope(ScopeBounds.Thread, ScopeNesting.Allowed))
            using (StubScope scope2 = new StubScope(ScopeBounds.Thread, ScopeNesting.Allowed))
            {
                Assert.AreEqual(scope2, StubScope.Current);
            }
        }

        [Test]
        public void ScopeAvailableInOtherMethods()
        {
            using (StubScope scope = new StubScope(ScopeBounds.Thread, ScopeNesting.Allowed))
            {
                AssertCurrentScopeIsNotNull();
            }
        }

        [Test]
        public void ThreadBoundScopeNotAvailableOnAnotherThread_WhenNestingAllowed()
        {
            bool isAvailable = true;

            Action assertCurrentScopeIsNull = delegate()
            {
                isAvailable = StubScope.Current != null;
            };

            using (StubScope scope = new StubScope(ScopeBounds.Thread, ScopeNesting.Allowed))
            {
                IAsyncResult result = assertCurrentScopeIsNull.BeginInvoke(null, null);

                result.AsyncWaitHandle.WaitOne();
                assertCurrentScopeIsNull.EndInvoke(result);
            }

            Assert.IsFalse(isAvailable);
        }

        [Test]
        public void AppDomainBoundScopeIsAvailableOnAnotherThread_WhenNestingAllowed()
        {
            bool isAvailable = false;

            Action assertCurrentScopeIsNull = delegate()
            {
                isAvailable = StubScope.Current != null;
            };

            using (StubScope scope = new StubScope(ScopeBounds.AppDomain, ScopeNesting.Allowed))
            {
                IAsyncResult result = assertCurrentScopeIsNull.BeginInvoke(null, null);

                result.AsyncWaitHandle.WaitOne();
                assertCurrentScopeIsNull.EndInvoke(result);
            }

            Assert.IsTrue(isAvailable);
        }

        [Test]
        public void Dispose_ViaDisposedNestedScope_DoesNotChangeCurrent()
        {
            using (var parent = new StubScope(ScopeBounds.Thread, ScopeNesting.Allowed))
            using (var child = new StubScope(ScopeBounds.Thread, ScopeNesting.Allowed))
            {
                child.Dispose();
                Assert.IsTrue(object.ReferenceEquals(parent, StubScope.Current));

                child.Dispose();
                Assert.IsTrue(object.ReferenceEquals(parent, StubScope.Current));
            }
        }

        private void AssertCurrentScopeIsNotNull()
        {
            Assert.IsNotNull(StubScope.Current);
        }
    }
}
