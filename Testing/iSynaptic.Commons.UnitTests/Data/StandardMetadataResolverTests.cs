using System;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;

namespace iSynaptic.Commons.Data
{
    [TestFixture]
    public class StandardMetadataResolverTests
    {

        [Test]
        public void Resolve_ThruMetadataClass_ReturnsValue()
        {
            var resolver = new StandardMetadataResolver(new TestMetadataBindingModule());

            Metadata.SetResolver(resolver);

            var value = StringMetadata.MaxLength.Get();
            Assert.AreEqual(42, value);
        }

        [Test]
        public void Resolve_WithMatchingBinding_ReturnsValue()
        {
            var resolver = new StandardMetadataResolver(new TestMetadataBindingModule());

            var value = resolver.Resolve(StringMetadata.MaxLength, null, null);
            Assert.AreEqual(42, value);
        }

        [Test]
        public void Resolve_WithoutMatchingBinding_ReturnsDefault()
        {
            var resolver = new StandardMetadataResolver(new TestMetadataBindingModule());

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

        [Test]
        public void Resolve_WithAttributedProperty_ReturnsValue()
        {
            var resolver = new StandardMetadataResolver();
            Metadata.SetResolver(resolver);

            var value = StringMetadata.MaxLength.For<TestSubject>(x => x.FirstName);
            Assert.AreEqual(84, value);
        }

        [Test]
        public void Resolve_WithAttributedField_ReturnsValue()
        {
            var resolver = new StandardMetadataResolver();
            Metadata.SetResolver(resolver);

            var value = StringMetadata.MaxLength.For<TestSubject>(x => x.LastName);
            Assert.AreEqual(1764, value);
        }

        [Test]
        public void Resolve_WithSurrogate_ReturnsValue()
        {
            var resolver = new StandardMetadataResolver();
            Metadata.SetResolver(resolver);

            var value = StringMetadata.MaxLength.For<TestSubject>(x => x.MiddleName);
            Assert.AreEqual(74088, value);
        }

        [Test]
        public void Resolve_WithAmbiguousBindings_ThrowsException()
        {
            var resolver = new StandardMetadataResolver(new TestMetadataBindingModule(), new TestMetadataBindingModule());
            Metadata.SetResolver(resolver);

            Assert.Throws<InvalidOperationException>(() => StringMetadata.MaxLength.Get());
        }
    }
}
