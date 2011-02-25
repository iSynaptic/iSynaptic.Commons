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
            MetadataDeclaration.SetResolver(null);
            Ioc.SetDependencyResolver(null);
        }

        [Test]
        public void LazyMetadata_ViaParameterlessConstructor_UsesTypeDeclaration()
        {
            var resolver = new StandardMetadataResolver();
            resolver.Bind(MetadataDeclaration<int>.TypeDeclaration, 42);

            MetadataDeclaration.SetResolver(resolver);

            var lazy = new LazyMetadata<int>();
            int value = lazy;

            Assert.AreEqual(42, value);
        }

        [Test]
        public void LazyMetadata_ViaDeclaration()
        {
            var resolver = new StandardMetadataResolver();
            resolver.Bind(StringMetadata.MaxLength, 42);

            MetadataDeclaration.SetResolver(resolver);

            Assert.AreEqual(42, StringMetadata.MaxLength.LazyGet());
        }

        [Test]
        public void LazyMetadata_ViaDeclarationSubjectType()
        {
            var resolver = new StandardMetadataResolver();
            resolver.Bind(StringMetadata.MaxLength)
                .For<string>()
                .To(42);

            MetadataDeclaration.SetResolver(resolver);

            Assert.AreEqual(42, StringMetadata.MaxLength.LazyFor<string>());
        }

        [Test]
        public void LazyMetadata_ViaDeclarationSubject()
        {
            string subject = "Hello, World!";

            var resolver = new StandardMetadataResolver();
            resolver.Bind(StringMetadata.MaxLength)
                .For(subject)
                .To(42);

            MetadataDeclaration.SetResolver(resolver);

            Assert.AreEqual(42, StringMetadata.MaxLength.LazyFor(subject));
        }

        [Test]
        public void LazyMetadata_ViaDeclarationMember()
        {
            Expression<Func<string, object>> expression = x => x.Length;

            var resolver = new StandardMetadataResolver();
            resolver.Bind(IntegerMetadata.MinValue)
                .For(expression)
                .To(42);

            MetadataDeclaration.SetResolver(resolver);

            Assert.AreEqual(42, IntegerMetadata.MinValue.LazyFor(expression));
        }

        [Test]
        public void LazyMetadata_ViaDeclarationSubjectMember()
        {
            string subject = "Hello, World!";
            Expression<Func<string, object>> expression = x => x.Length;

            var resolver = new StandardMetadataResolver();
            resolver.Bind(IntegerMetadata.MinValue)
                .For(subject, expression)
                .To(42);

            MetadataDeclaration.SetResolver(resolver);

            Assert.AreEqual(42, IntegerMetadata.MinValue.LazyFor(subject, expression));
        }
    }
}
