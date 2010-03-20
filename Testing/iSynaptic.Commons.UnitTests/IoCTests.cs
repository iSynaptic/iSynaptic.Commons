using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iSynaptic.Commons.Testing.RhinoMocks;
using iSynaptic.Commons.Testing.NUnit;

using NUnit.Framework;
using Rhino.Mocks;
using Rhino.Mocks.Constraints;

namespace iSynaptic.Commons
{
    [TestFixture]
    [RequiresMocking]
    public class IoCTests : NUnitBaseTestFixture
    {
        [Test]
        public void Resolve_WithNoParameters_ReturnsExpectedValue()
        {
            var resolver = Mocks.Stub<IDependencyResolver>();
            resolver.Expect(x => x.Resolve(null, typeof(int), typeof(IoCTests)))
                .Return(42)
                .Repeat.Twice();

            Mocks.ReplayAll();
            IoC.SetDependencyResolver(resolver);

            Assert.AreEqual(42, IoC.Resolve<int>());
            Assert.AreEqual(42, IoC.Resolve(typeof(int)));
        }

        [Test]
        public void Resolve_WithKey_ReturnsExpectedValue()
        {
            var resolver = Mocks.Stub<IDependencyResolver>();
            resolver.Stub(x => x.Resolve("ultimateAnswerTimesTwo", typeof(int), typeof(IoCTests)))
                .Return(84)
                .Repeat.Twice();

            Mocks.ReplayAll();
            IoC.SetDependencyResolver(resolver);

            Assert.AreEqual(84, IoC.Resolve<int>("ultimateAnswerTimesTwo"));
            Assert.AreEqual(84, IoC.Resolve(typeof(int), "ultimateAnswerTimesTwo"));
        }

        [Test]
        public void Resolve_WithKeyAndRequestingType_ReturnsExpectedValue()
        {
            var resolver = Mocks.Stub<IDependencyResolver>();
            resolver.Stub(x => x.Resolve("ultimateAnswerTimesThree", typeof(int), typeof(string)))
                .Return(126)
                .Repeat.Twice();

            Mocks.ReplayAll();
            IoC.SetDependencyResolver(resolver);

            Assert.AreEqual(126, IoC.Resolve<int>("ultimateAnswerTimesThree", typeof(string)));
            Assert.AreEqual(126, IoC.Resolve(typeof(int), "ultimateAnswerTimesThree", typeof(string)));
        }

        public MockRepository Mocks { get; set; }
    }
}
