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
        public void CreateScope()
        {
            using (StubScope scope = new StubScope())
            {
                Assert.IsNotNull(StubScope.Current, "Scope was not created correctly.");
                Assert.AreEqual(scope, StubScope.Current, "Incorrect current scope.");
            }
        }

        [Test]
        public void ScopeTearsDown()
        {
            Assert.IsNull(StubScope.Current, "Current scope was not null to begin with.");

            using (StubScope scope = new StubScope())
            {
                Assert.IsNotNull(StubScope.Current, "Scope was not created correctly.");
                Assert.AreEqual(scope, StubScope.Current, "Incorrect current scope.");
            }

            Assert.IsNull(StubScope.Current, "Current scope was not null to end with.");
        }

        //[Test]
        //[ExpectedException(typeof(ApplicationException))]
        //public void NestedScopesNotAllowed()
        //{
        //    using (StubScope scope = new StubScope())
        //    {
        //        using (StubScope scope2 = new StubScope())
        //        {
        //        }
        //    }
        //}
    }
}
