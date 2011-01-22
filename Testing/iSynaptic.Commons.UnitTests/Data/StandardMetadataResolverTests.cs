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
        [SetUp]
        public void BeforeTest()
        {
            TestSubjectMetadataSurrogate.ShouldYieldInstanceMetadata = false;
        }

        [Test]
        public void Resolve_WithModuleProvidedMatchingBinding_ReturnsValue()
        {
            var resolver = new StandardMetadataResolver(new TestMetadataBindingModule());
            Metadata.SetResolver(resolver);

            int value = StringMetadata.MaxLength;
            Assert.AreEqual(42, value);
        }

        [Test]
        public void Resolve_AfterUnloadingModule_ReturnsDefault()
        {
            var module = new TestMetadataBindingModule();
            var resolver = new StandardMetadataResolver(module);
            Metadata.SetResolver(resolver);

            int value = StringMetadata.MaxLength;
            Assert.AreEqual(42, value);

            resolver.UnloadModule(module);
            
            value = StringMetadata.MaxLength;
            Assert.AreEqual(StringMetadata.MaxLength.Default, value);
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

        [Test]
        public void Resolve_WithAttributedType_ReturnsValue()
        {
            var resolver = new StandardMetadataResolver();
            Metadata.SetResolver(resolver);

            var value = CommonMetadata.Description.For<TestSubject>();
            Assert.AreEqual("Test Subject", value);
        }

        [Test]
        public void Resolve_WithModuleThatOverridesAttributeMetadata_ReturnsValue()
        {
            var resolver = new StandardMetadataResolver(new TestMetadataBindingModule());
            Metadata.SetResolver(resolver);

            var value = CommonMetadata.Description.For<TestSubject>();
            Assert.AreEqual("Overriden Description", value);
        }

        [Test]
        public void Resolve_AgainstSubjectInstanceWithAttributedType_ReturnsValue()
        {
            var resolver = new StandardMetadataResolver();
            Metadata.SetResolver(resolver);

            var subject = new TestSubject();

            var value = CommonMetadata.Description.For(subject);
            Assert.AreEqual("Test Subject", value);
        }

        [Test]
        public void Resolve_AgainstSpecificInstance_WorksCorrectly()
        {
            var resolver = new StandardMetadataResolver();
            Metadata.SetResolver(resolver);

            TestSubjectMetadataSurrogate.ShouldYieldInstanceMetadata = true;

            var value = CommonMetadata.Description.For(TestSubjectMetadataSurrogate.Subject);
            Assert.AreEqual("Special Instance Description", value);
        }

        [Test]
        public void Resolve_AgainstArbitraryInstance_YieldsAttributeMetadata()
        {
            var resolver = new StandardMetadataResolver();
            Metadata.SetResolver(resolver);

            TestSubjectMetadataSurrogate.ShouldYieldInstanceMetadata = false;

            var value = CommonMetadata.Description.For(new TestSubject());
            Assert.AreEqual("Test Subject", value);
        }

        [Test]
        public void Resolve_AgainstArbitraryInstance_YieldsSurrogateMetadata()
        {
            var resolver = new StandardMetadataResolver();
            Metadata.SetResolver(resolver);

            TestSubjectMetadataSurrogate.ShouldYieldInstanceMetadata = true;

            var value = CommonMetadata.Description.For(new TestSubject());
            Assert.AreEqual("Surrogate Description", value);
        }

        [Test]
        public void Resolve_AgainstSpecificInstanceWhenPredicateReturnsFalse_YieldsAttributeMetadata()
        {
            var resolver = new StandardMetadataResolver();
            Metadata.SetResolver(resolver);

            TestSubjectMetadataSurrogate.ShouldYieldInstanceMetadata = false;

            var value = CommonMetadata.Description.For(TestSubjectMetadataSurrogate.Subject);
            Assert.AreEqual("Test Subject", value);
        }

        [Test]
        public void Resolve_AgainstArbitraryDerivedInstance_YieldsSurrogateMetadata()
        {
            var resolver = new StandardMetadataResolver();
            Metadata.SetResolver(resolver);

            TestSubjectMetadataSurrogate.ShouldYieldInstanceMetadata = true;

            var value = CommonMetadata.Description.For(new DerivedTestSubject());
            Assert.AreEqual("Surrogate Description", value);
        }

        [Test]
        public void Resolve_WithDerivedInstance_YieldsMostDerivedBindingsMetadata()
        {
            var module = new MetadataBindingModule();
            module.Bind(CommonMetadata.Description)
                .For<DerivedTestSubject>()
                .To("Derived Surrogate Description");

            var resolver = new StandardMetadataResolver(module);
            Metadata.SetResolver(resolver);

            TestSubjectMetadataSurrogate.ShouldYieldInstanceMetadata = true;

            var value = CommonMetadata.Description.For(new DerivedTestSubject());
            Assert.AreEqual("Derived Surrogate Description", value);
        }
    }
}
