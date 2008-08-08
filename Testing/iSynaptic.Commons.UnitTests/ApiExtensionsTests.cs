using System;
using System.Collections.Generic;
using System.Text;
using MbUnit.Framework;
using Rhino.Mocks;

namespace iSynaptic.Commons.UnitTests
{
    [TestFixture]
    public class ApiExtensionsTests
    {
        [Test]
        public void ResolveWithoutContext()
        {
            MockRepository mocks = new MockRepository();

            var resolver = mocks.CreateMock<IDependencyResolver>();
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

            var resolver = mocks.CreateMock<IDependencyResolver>();
            Expect.Call(resolver.Resolve(typeof(int), context)).Return(1);
            mocks.Replay(resolver);

            resolver.Resolve<int>(context);

            mocks.Verify(resolver);
        }
    }
}
