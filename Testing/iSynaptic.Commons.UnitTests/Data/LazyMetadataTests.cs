using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;

namespace iSynaptic.Commons.Data
{
    [TestFixture]
    public class LazyMetadataTests
    {
        [SetUp]
        public void BeforeTest()
        {
            Metadata.SetResolver(null);
            Ioc.SetDependencyResolver(null);
        }

        [Test]
        public void LazyMetadata_ViaParameterlessConstructor_UsesTypeDeclaration()
        {
            var resolver = MockRepository.GenerateStub<IMetadataResolver>();
            resolver.Stub(x => x.Resolve<int, object>(MetadataDeclaration<int>.TypeDeclaration, null, null))
                .Return(42);

            Metadata.SetResolver(resolver);

            var lazy = new LazyMetadata<int>();
            int value = lazy;

            Assert.AreEqual(42, value);
        }

        [Test]
        public void LazyMetadata_ViaDeclaration()
        {
            var resolver = MockRepository.GenerateStub<IMetadataResolver>();
            resolver.Stub(x => x.Resolve<int, object>(StringMetadata.MaxLength, null, null))
                .Return(42);

            Metadata.SetResolver(resolver);

            Assert.AreEqual(42, StringMetadata.MaxLength.LazyGet());
        }

        [Test]
        public void LazyMetadata_ViaDeclarationSubjectType()
        {
            var resolver = MockRepository.GenerateStub<IMetadataResolver>();
            resolver.Stub(x => x.Resolve(StringMetadata.MaxLength, typeof(string), null))
                .Return(42);

            Metadata.SetResolver(resolver);

            Assert.AreEqual(42, StringMetadata.MaxLength.LazyFor<string>());
        }

        [Test]
        public void LazyMetadata_ViaDeclarationSubject()
        {
            string subject = "Hello, World!";

            var resolver = MockRepository.GenerateStub<IMetadataResolver>();
            resolver.Stub(x => x.Resolve(StringMetadata.MaxLength, subject, null))
                .Return(42);

            Metadata.SetResolver(resolver);

            Assert.AreEqual(42, StringMetadata.MaxLength.LazyFor(subject));
        }

        [Test]
        public void LazyMetadata_ViaDeclarationMember()
        {
            Expression<Func<string, object>> expression = x => x.Length;

            var member = expression.ExtractMemberInfoForMetadata();

            var resolver = MockRepository.GenerateStub<IMetadataResolver>();
            resolver.Stub(x => x.Resolve(IntegerMetadata.MinValue, typeof(string), member))
                .Return(42);

            Metadata.SetResolver(resolver);

            Assert.AreEqual(42, IntegerMetadata.MinValue.LazyFor(expression));
        }

        [Test]
        public void LazyMetadata_ViaDeclarationSubjectMember()
        {
            string subject = "Hello, World!";
            Expression<Func<string, object>> expression = x => x.Length;

            var member = expression.ExtractMemberInfoForMetadata();

            var resolver = MockRepository.GenerateStub<IMetadataResolver>();
            resolver.Stub(x => x.Resolve(IntegerMetadata.MinValue, subject, member))
                .Return(42);

            Metadata.SetResolver(resolver);

            Assert.AreEqual(42, IntegerMetadata.MinValue.LazyFor(subject, expression));
        }
    }
}
