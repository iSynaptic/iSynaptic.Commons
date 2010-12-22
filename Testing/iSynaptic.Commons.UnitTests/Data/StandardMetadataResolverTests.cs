using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;

namespace iSynaptic.Commons.Data
{
    [TestFixture]
    public class StandardMetadataResolverTests
    {
        private class TestModule : MetadataBindingModule
        {
            public TestModule()
            {
                Bind(StringMetadata.MaxLength, 42);
            }
        }

        [Test]
        public void Resolve_ThruMetadataClass_ReturnsValue()
        {
            var resolver = new StandardMetadataResolver(new TestModule());

            Metadata.SetMetadataResolver(resolver);

            var value = Metadata.Get(StringMetadata.MaxLength);
            Assert.AreEqual(42, value);
        }

        [Test]
        public void Resolve_WithMatchingBinding_ReturnsValue()
        {
            var resolver = new StandardMetadataResolver(new TestModule());

            var value = resolver.Resolve(StringMetadata.MaxLength, null, null);
            Assert.AreEqual(42, value);
        }

        [Test]
        public void Resolve_WithoutMatchingBinding_ReturnsDefault()
        {
            var resolver = new StandardMetadataResolver(new TestModule());

            var value = resolver.Resolve(new IntegerMetadataDeclaration(-1, 42, 7), null, null);
            Assert.AreEqual(7, value);
        }

        [Test]
        public void Resolve_WithNoBindings_ReturnsDefault()
        {
            var resolver = new StandardMetadataResolver();

            var value = resolver.Resolve(new IntegerMetadataDeclaration(-1, 42, 7), null, null);
            Assert.AreEqual(7, value);
        }
    }
}
