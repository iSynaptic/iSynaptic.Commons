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

        [SurrogateMetadataBindingSource(typeof(TestSubject))]
        public class TestSubjectMetadata : IMetadataBindingSource
        {
            public IEnumerable<IMetadataBinding<TMetadata>> GetBindingsFor<TMetadata>(MetadataRequest<TMetadata> request)
            {
                if(request.Declaration != StringMetadata.MaxLength)
                    yield break;

                if((Type)request.Subject != typeof(TestSubject))
                    yield break;

                if(request.Member.Name != "MiddleName")
                    yield break;

                yield return new MetadataBinding<TMetadata>(request.Declaration, 74088);
            }
        }

        private class TestSubject
        {
            [MaxLength(84)]
            public string FirstName { get; set; }

            [MaxLength(1764)]
            public string LastName = null;

            public string MiddleName { get; set; }
        }

        [Test]
        public void Resolve_ThruMetadataClass_ReturnsValue()
        {
            var resolver = new StandardMetadataResolver(new TestModule());

            Metadata.SetResolver(resolver);

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

        [Test]
        public void Resolve_WithAttributedProperty_ReturnsValue()
        {
            var resolver = new StandardMetadataResolver();
            Metadata.SetResolver(resolver);

            var value = Metadata<TestSubject>.Get(StringMetadata.MaxLength, x => x.FirstName);
            Assert.AreEqual(84, value);
        }

        [Test]
        public void Resolve_WithAttributedField_ReturnsValue()
        {
            var resolver = new StandardMetadataResolver();
            Metadata.SetResolver(resolver);

            var value = Metadata<TestSubject>.Get(StringMetadata.MaxLength, x => x.LastName);
            Assert.AreEqual(1764, value);
        }

        [Test]
        public void Resolve_WithSurrogate_ReturnsValue()
        {
            var resolver = new StandardMetadataResolver();
            Metadata.SetResolver(resolver);

            var value = Metadata<TestSubject>.Get(StringMetadata.MaxLength, x => x.MiddleName);
            Assert.AreEqual(74088, value);
        }
    }
}
