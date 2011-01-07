using System;
using System.Linq;
using System.Text;
using iSynaptic.Commons.Data.MetadataDeclarations;
using NUnit.Framework;
using Rhino.Mocks;

namespace iSynaptic.Commons.Data
{
    [TestFixture]
    public class StandardMetadataResolverTests
    {
        [Test]
        public void Resolve_WithModuleProvidedMatchingBinding_ReturnsValue()
        {
            var resolver = new StandardMetadataResolver(new TestMetadataBindingModule());
            Metadata.SetResolver(resolver);

            int value = StringMetadata.MaxLength;
            Assert.AreEqual(42, value);
        }

        [Test]
        public void Resolve_WithAttributedProperty_ReturnsValue()
        {
            var resolver = new StandardMetadataResolver();
            Metadata.SetResolver(resolver);

            var minLength = StringMetadata.MinLength.For<TestSubject>(x => x.FirstName);
            Assert.AreEqual(21, minLength);

            var maxLength = StringMetadata.MaxLength.For<TestSubject>(x => x.FirstName);
            Assert.AreEqual(84, maxLength);

            var description = CommonMetadata.Description.For<TestSubject>(x => x.FirstName);
            Assert.AreEqual("First Name", description);
        }

        [Test]
        public void Resolve_WithAttributedField_ReturnsValue()
        {
            var resolver = new StandardMetadataResolver();
            Metadata.SetResolver(resolver);

            var allMetadata = StringMetadata.All.For<TestSubject>(x => x.LastName);

            Assert.AreEqual(7, allMetadata.MinimumLength);
            Assert.AreEqual(1764, allMetadata.MaximumLength);
            Assert.AreEqual("Last Name", allMetadata.Description);
        }

        [Test]
        public void Resolve_WithSurrogate_ReturnsValue()
        {
            var resolver = new StandardMetadataResolver();
            Metadata.SetResolver(resolver);

            var value = StringMetadata.MaxLength.For<TestSubject>(x => x.MiddleName);
            Assert.AreEqual(74088, value);
        }
    }
}
