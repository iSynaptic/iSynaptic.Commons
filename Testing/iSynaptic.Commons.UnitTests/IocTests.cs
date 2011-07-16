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
            var resolver = new DependencyResolver(x =>
                x is ISymbol<int>
                ? 42
                : Maybe<object>.NoValue);

            Ioc.SetDependencyResolver(resolver);

            Assert.AreEqual(42, Ioc.Resolve<int>());
            Assert.AreEqual(42, Ioc.Resolve(typeof(int)));
        }

        [Test]
        public void Resolve_WithKeyAndRequestingType_ReturnsExpectedValue()
        {
            var resolver = new DependencyResolver(x =>
                x is INamedSymbol<int> && ((INamedSymbol<int>)x).Name == "ultimateAnswerTimesThree"
                ? 126
                : Maybe<object>.NoValue);

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
