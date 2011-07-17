using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace iSynaptic.Commons
{
    [TestFixture]
    public class DependencyResolverTests
    {
        [Test]
        public void Construction_WithNullResolutionStrategy_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new DependencyResolver(null));
        }

        [Test]
        public void Resolve_WithAbritraryParameters_CallsResolutionStrategyCorrectly()
        {
            string name = null;
            Type dependencyType = null;

            Func<ISymbol, Maybe<object>> strategy = symbol =>
                                                {
                                                    name = ((IDependencySymbol) symbol).Name;
                                                    dependencyType = ((IDependencySymbol) symbol).DependencyType;
                                                    return Maybe<object>.NoValue;
                                                };

            var resolver = new DependencyResolver(strategy);
            resolver.Resolve(typeof(IDisposable), "Foo");

            Assert.AreEqual("Foo", name);
            Assert.AreEqual(typeof(IDisposable), dependencyType);

        }

        [Test]
        public void Resolve_WithAbritraryParameters_ReturnsResultCorrectly()
        {
            Func<ISymbol, Maybe<object>> strategy = _ => "Baz".ToMaybe<object>();

            var resolver = new DependencyResolver(strategy);
            var result = resolver.Resolve(typeof (IDisposable), "Foo");

            Assert.AreEqual("Baz", result);
        }
    }
}
