using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;

namespace iSynaptic.Commons.Data
{
    [TestFixture]
    public class ExodataResolverTests
    {
        [Test]
        public void TryResolve_WithNoBindings_ReturnsNoValue()
        {
            var symbol = new Symbol();
            var source = new ExodataBindingModule();

            var resolver = new ExodataResolver(new[] { source });

            Maybe<int> value = resolver.TryResolve<int, object, object>(symbol, Maybe<object>.NoValue, Maybe<object>.NoValue, null);

            Assert.AreEqual(Maybe<int>.NoValue, value);
        }

        [Test]
        public void TryResolve_WithAmbiguousBindingSelection_YieldsAmbiguousExodataBindingsException()
        {
            var symbol = new Symbol();
            var source = new ExodataBindingModule();
            source.Bind(symbol, 42);
            source.Bind(symbol, 42);

            var resolver = new ExodataResolver(new[] { source });

            Maybe<int> result = resolver.TryResolve<int, object, object>(symbol, Maybe<object>.NoValue, Maybe<object>.NoValue, null);

            var exception = result.Exception as AmbiguousExodataBindingsException;

            Assert.IsNotNull(exception);
            Assert.AreEqual(2, exception.Bindings.Count());
        }

        [Test]
        public void TryResolve_WithNonAmbiguousBindingSelection_YieldsAmbiguousExodataBindingsException()
        {
            var symbol = new Symbol();
            var source = new ExodataBindingModule();
            source.Bind(symbol, 42);

            var resolver = new ExodataResolver(new[] { source });

            Maybe<int> result = resolver.TryResolve<int, object, object>(symbol, Maybe<object>.NoValue, Maybe<object>.NoValue, null);

            Assert.AreEqual(42, result.Value);
        }
    }
}
