using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;
using Rhino.Mocks;

namespace iSynaptic.Commons
{
    [TestFixture]
    public class IocTests
    {
        [Test]
        public void Resolve_WithNoParameters_ReturnsExpectedValue()
        {
            var resolver = MockRepository.GenerateStub<IDependencyResolver>();
            resolver.Stub(x => x.Resolve(null, typeof(int), typeof(IocTests)))
                .Return(42)
                .Repeat.Twice();

            Ioc.SetDependencyResolver(resolver);

            Assert.AreEqual(42, Ioc.Resolve<int>());
            Assert.AreEqual(42, Ioc.Resolve(typeof(int)));
        }

        [Test]
        public void Resolve_WithKey_ReturnsExpectedValue()
        {
            var resolver = MockRepository.GenerateStub<IDependencyResolver>();
            resolver.Stub(x => x.Resolve("ultimateAnswerTimesTwo", typeof(int), typeof(IocTests)))
                .Return(84)
                .Repeat.Twice();

            Ioc.SetDependencyResolver(resolver);

            Assert.AreEqual(84, Ioc.Resolve<int>("ultimateAnswerTimesTwo"));
            Assert.AreEqual(84, Ioc.Resolve(typeof(int), "ultimateAnswerTimesTwo"));
        }

        [Test]
        public void Resolve_WithKeyAndRequestingType_ReturnsExpectedValue()
        {
            var resolver = MockRepository.GenerateStub<IDependencyResolver>();
            resolver.Stub(x => x.Resolve("ultimateAnswerTimesThree", typeof(int), typeof(string)))
                .Return(126)
                .Repeat.Twice();

            Ioc.SetDependencyResolver(resolver);

            Assert.AreEqual(126, Ioc.Resolve<int>("ultimateAnswerTimesThree", typeof(string)));
            Assert.AreEqual(126, Ioc.Resolve(typeof(int), "ultimateAnswerTimesThree", typeof(string)));
        }

        [Test]
        public void Resolve_WithNoResolver_ReturnsNull()
        {
            Ioc.SetDependencyResolver((IDependencyResolver)null);

            Assert.IsNull(Ioc.Resolve<IDisposable>());
        }
    }
}
