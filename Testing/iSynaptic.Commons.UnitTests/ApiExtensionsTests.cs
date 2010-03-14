using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;

namespace iSynaptic.Commons
{
    [TestFixture]
    public class ApiExtensionsTests
    {
        [Test]
        public void ResolveWithoutContext()
        {
            MockRepository mocks = new MockRepository();

            var resolver = mocks.StrictMock<IDependencyResolver>();
            Expect.Call(resolver.Resolve(typeof(int), null)).Return(1);
            mocks.Replay(resolver);

            resolver.Resolve<int>();

            mocks.Verify(resolver);
        }

        [Test]
        public void ResolveWithContext()
        {
            object context = new object();

            MockRepository mocks = new MockRepository();

            var resolver = mocks.StrictMock<IDependencyResolver>();
            Expect.Call(resolver.Resolve(typeof(int), context)).Return(1);
            mocks.Replay(resolver);

            resolver.Resolve<int>(context);

            mocks.Verify(resolver);
        }

        [Test]
        public void ResolveOnNullResolver()
        {
            IDependencyResolver resolver = null;
            
            Assert.Throws<ArgumentNullException>(() => resolver.Resolve<int>());
        }

        [Test]
        public void ResolveOnNullResolverWithContext()
        {
            object context = new object();
            IDependencyResolver resolver = null;
            
            Assert.Throws<ArgumentNullException>(() => resolver.Resolve<int>(context));
        }
    }
}
