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
        public void CreateNestedScope()
        {
            using (StubNestableScope scope = new StubNestableScope())
            {
                Assert.IsNotNull(StubNestableScope.Current, "Current nestable scope not null.");
                Assert.AreEqual(scope, StubNestableScope.Current, "Current nestable scope is incorrect.");
            }
        }
    }
}
