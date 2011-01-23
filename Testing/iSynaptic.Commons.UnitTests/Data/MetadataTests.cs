using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using iSynaptic.Commons.Data.MetadataDeclarations;
using NUnit.Framework;
using Rhino.Mocks;
using Is = Rhino.Mocks.Constraints.Is;

namespace iSynaptic.Commons.Data
{
    [TestFixture]
    public class MetadataTests
    {
        [SetUp]
        public void BeforeTest()
        {
            Metadata.SetResolver(null);
            Ioc.SetDependencyResolver(null);
        }

        [Test]
        public void Get_ByTypeOnly_UsesTypeDeclaration()
        {
            var resolver = MockRepository.GenerateStub<IMetadataResolver>();
            resolver.Stub(x => x.Resolve<int, object>(MetadataDeclaration<int>.TypeDeclaration, null, null))
                .IgnoreArguments()
                .Return(42);

            Metadata.SetResolver(resolver);

            Assert.AreEqual(42, Metadata.Get<int>());
        }

        [Test]
        public void Get_WithValidArguments_ReturnsMetadataResolverResults()
        {
            var maxLength = new MetadataDeclaration<int>(7);

            var resolver = MockRepository.GenerateStub<IMetadataResolver>();
            resolver.Stub(x => x.Resolve(maxLength, Maybe<string>.NoValue, null))
                .Return(42);

            Metadata.SetResolver(resolver);

            var value = maxLength.For<string>();
            Assert.AreEqual(42, value);
        }

        [Test]
        public void Resolve_WithNullDeclaration_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => 
                Metadata.Resolve<int, object>(null, null, null));
        }

        [Test]
        public void Get_WithNoMetadataResolver_ReturnsDefault()
        {
            var maxLength = new MetadataDeclaration<int>(7);

            var value = maxLength.For<string>();
            Assert.AreEqual(7, value);
        }

        [Test]
        public void Get_WithNoMetadataResolverAndBadDefault_ThrowsWrappedException()
        {
            var maxLengthWithBadDefault = new ComparableMetadataDeclaration<int>(1, 10, 42);

            Assert.That(() => { maxLengthWithBadDefault.For<string>(); },
                Throws
                    .InstanceOf<InvalidOperationException>().And
                    .InnerException
                        .InstanceOf<MetadataValidationException<int>>());
        }

        [Test]
        public void Get_WithNoMetadataResolver_UsesResolverFromIoc()
        {
            var maxLength = new MetadataDeclaration<int>(7);

            var metadataResolver = MockRepository.GenerateStub<IMetadataResolver>();
            metadataResolver.Stub(x => x.Resolve(maxLength, Maybe<string>.NoValue, null))
                .Return(42);

            Ioc.SetDependencyResolver(new DependencyResolver((k, d, r) => metadataResolver));

            var value = maxLength.For<string>();
            Assert.AreEqual(42, value);
        }

        [Test]
        public void Get_OnNonGenericMetadataWithDeclaration_ProvidesAllArgumentsToResolver()
        {
            var maxLength = new MetadataDeclaration<int>(7);

            var resolver = MockRepository.GenerateMock<IMetadataResolver>();
            resolver.Expect(x => x.Resolve<int, object>(maxLength, null, null))
                .Return(42);

            Metadata.SetResolver(resolver);

            maxLength.Get();
            resolver.VerifyAllExpectations();
        }

        [Test]
        public void Get_OnGenericMetadataWithDeclaration_ProvidesAllArgumentsToResolver()
        {
            var maxLength = new MetadataDeclaration<int>(7);

            var resolver = MockRepository.GenerateMock<IMetadataResolver>();
            resolver.Expect(x => x.Resolve(maxLength, Maybe<string>.NoValue, null))
                .Return(42);

            Metadata.SetResolver(resolver);

            maxLength.For<string>();
            resolver.VerifyAllExpectations();
        }

        [Test]
        public void Get_OnGenericMetadataWithDeclarationAndSubject_ProvidesAllArgumentsToResolver()
        {
            var maxLength = new MetadataDeclaration<int>(7);
            string subject = "Hello, World!";

            var resolver = MockRepository.GenerateMock<IMetadataResolver>();
            resolver.Expect(x => x.Resolve<int, string>(maxLength, subject, null))
                .Return(42);

            Metadata.SetResolver(resolver);

            maxLength.For(subject);
            resolver.VerifyAllExpectations();
        }

        [Test]
        public void Get_OnGenericMetadataWithDeclarationAndMember_ProvidesAllArgumentsToResolver()
        {
            Expression<Func<string, object>> expression = x => x.Length;
            var member = expression.ExtractMemberInfoForMetadata();

            var maxLength = new MetadataDeclaration<int>(7);

            var resolver = MockRepository.GenerateMock<IMetadataResolver>();
            resolver.Expect(x => x.Resolve(maxLength, Maybe<string>.NoValue, member))
                .Return(42);

            Metadata.SetResolver(resolver);

            maxLength.For<string>(expression);
            resolver.VerifyAllExpectations();
        }

        [Test]
        public void Get_OnGenericMetadataWithDeclarationSubjectAndMember_ProvidesAllArgumentsToResolver()
        {
            var maxLength = new MetadataDeclaration<int>(7);
            string subject = "Hello, World!";

            Expression<Func<string, object>> expression = x => x.Length;
            var member = expression.ExtractMemberInfoForMetadata();

            var resolver = MockRepository.GenerateMock<IMetadataResolver>();
            resolver.Expect(x => x.Resolve<int, string>(maxLength, subject, member))
                .Return(42);

            Metadata.SetResolver(resolver);

            maxLength.For(subject, expression);
            resolver.VerifyAllExpectations();
        }

        [Test]
        public void Get_OnNestedProperty_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
                StringMetadata.MaxLength
                    .For<TestSubject>(x => x.MiddleName.Length));
        }

        [Test]
        public void Get_OnMethodMember_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
                StringMetadata.MinLength
                    .For<TestSubject>(x => x.ToString()));
        }

        [Test]
        public void Get_OnMethodNestedMember_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
                StringMetadata.MinLength
                    .For<TestSubject>(x => x.ToString().Length));
        }
    }
}
