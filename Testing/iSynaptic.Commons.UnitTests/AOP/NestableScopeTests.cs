﻿using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;

using iSynaptic.Commons.AOP;

namespace iSynaptic.Commons.AOP
{
    [TestFixture]
    public class NestableScopeTests
    {
        [Test]
        public void NestingAppDomainWithinThread()
        {
            Assert.Throws<ApplicationException>(() =>
            {
                using (new StubNestableScope(ScopeBounds.Thread))
                using (new StubNestableScope(ScopeBounds.AppDomain))
                {

                }
            });
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
                AssertCurrentScopeIsNotNull();
            }
        }

        [Test]
        public void ThreadBoundScopeNotAvailableOnAnotherThread()
        {
            bool isAvailable = true;

            Action assertCurrentScopeIsNull = delegate()
            {
                isAvailable = StubNestableScope.Current != null;
            };

            using (StubNestableScope scope = new StubNestableScope(ScopeBounds.Thread))
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
                isAvailable = StubNestableScope.Current != null;
            };

            using (StubNestableScope scope = new StubNestableScope(ScopeBounds.AppDomain))
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
            using(var parent = new StubNestableScope(ScopeBounds.Thread))
            using(var child = new StubNestableScope(ScopeBounds.Thread))
            {
                child.Dispose();
                Assert.IsTrue(object.ReferenceEquals(parent, StubNestableScope.Current));

                child.Dispose();
                Assert.IsTrue(object.ReferenceEquals(parent, StubNestableScope.Current));
            }
        }

        private void AssertCurrentScopeIsNotNull()
        {
            Assert.IsNotNull(StubNestableScope.Current);
        }
    }
}
