using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;
using Rhino.Mocks;
using Is = Rhino.Mocks.Constraints.Is;

namespace iSynaptic.Commons
{
    [TestFixture]
    public class IocTests
    {
        [Test]
        public void Resolve_WithNoParameters_ReturnsExpectedValue()
        {
            var resolver = MockRepository.GenerateStub<IDependencyResolver>();
            resolver.Stub(x => x.Resolve(null))
                .Constraints(Is.Matching<IDependencyDeclaration>(x => x.DependencyType == typeof(int)))
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
            resolver.Stub(x => x.Resolve(null))
                .Constraints(Is.Matching<INamedDependencyDeclaration>(x => x.DependencyType == typeof(int) &&
                                                                           x.Name == "ultimateAnswerTimesTwo"))
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
            resolver.Stub(x => x.Resolve(null))
                .Constraints(Is.Matching<INamedDependencyDeclaration>(x => x.DependencyType == typeof(int) &&
                                                                           x.Name == "ultimateAnswerTimesThree"))
                .Return(126)
                .Repeat.Twice();

            Ioc.SetDependencyResolver(resolver);

            Assert.AreEqual(126, Ioc.Resolve<int>("ultimateAnswerTimesThree"));
            Assert.AreEqual(126, Ioc.Resolve(typeof(int), "ultimateAnswerTimesThree"));
        }

        [Test]
        public void Resolve_WithNoResolver_ReturnsNull()
        {
            Ioc.SetDependencyResolver(null);

            Assert.IsNull(Ioc.Resolve<IDisposable>());
        }
    }
}
