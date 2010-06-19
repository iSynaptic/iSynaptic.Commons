using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iSynaptic.Commons.Testing.NUnit;
using NUnit.Framework;

namespace iSynaptic.Commons
{
    [TestFixture]
    public class DependencyResolverTests : NUnitBaseTestFixture
    {
        [Test]
        public void Construction_WithNullResolutionStrategy_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new DependencyResolver(null));
        }

        [Test]
        public void Resolve_WithAbritraryParameters_CallsResolutionStrategyCorrectly()
        {
            string key = null;
            Type dependencyType = null;
            Type requestingType = null;

            Func<string, Type, Type, object> strategy = (k, d, r) =>
                                                {
                                                    key = k;
                                                    dependencyType = d;
                                                    requestingType = r;
                                                    return null;
                                                };

            strategy("Foo", typeof (IDisposable), typeof (string));

            Assert.AreEqual("Foo", key);
            Assert.AreEqual(typeof(IDisposable), dependencyType);
            Assert.AreEqual(typeof(string), requestingType);

        }

        [Test]
        public void Resolve_WithAbritraryParameters_ReturnsResultCorrectly()
        {
            Func<string, Type, Type, object> strategy = (k, d, r) => "Baz";

            var result = strategy("Foo", typeof(IDisposable), typeof(string));

            Assert.AreEqual("Baz", result);
        }
    }
}
