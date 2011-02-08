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

            Func<IDependencyDeclaration, object> strategy = decl =>
                                                {
                                                    name = ((INamedDependencyDeclaration)decl).Name;
                                                    dependencyType = decl.DependencyType;
                                                    return null;
                                                };

            var resolver = new DependencyResolver(strategy);
            resolver.Resolve(typeof(IDisposable), "Foo");

            Assert.AreEqual("Foo", name);
            Assert.AreEqual(typeof(IDisposable), dependencyType);

        }

        [Test]
        public void Resolve_WithAbritraryParameters_ReturnsResultCorrectly()
        {
            Func<IDependencyDeclaration, object> strategy = decl => "Baz";

            var resolver = new DependencyResolver(strategy);
            var result = resolver.Resolve(typeof (IDisposable), "Foo");

            Assert.AreEqual("Baz", result);
        }
    }
}
