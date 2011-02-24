using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using iSynaptic.Commons.Data.MetadataDeclarations;
using NUnit.Framework;
using Rhino.Mocks;

namespace iSynaptic.Commons.Data
{
    [TestFixture]
    public class MetadataDeclarationTests
    {
        [SetUp]
        public void BeforeTest()
        {
            MetadataDeclaration.SetResolver(null);
            Ioc.SetDependencyResolver(null);
        }

        [Test]
        public void Resolve_ThruImplicitCastOperator_ReturnsValue()
        {
            var module = new MetadataBindingModule();
            module.Bind(StringMetadata.MaxLength, 42);

            var resolver = new StandardMetadataResolver(module);

            MetadataDeclaration.SetResolver(resolver);

            int value = StringMetadata.MaxLength;
            Assert.AreEqual(42, value);
        }

        [Test]
        public void Get_ByTypeOnly_UsesTypeDeclaration()
        {
            var module = new MetadataBindingModule();
            module.Bind(MetadataDeclaration<int>.TypeDeclaration, 42);

            var resolver = new StandardMetadataResolver(module);

            MetadataDeclaration.SetResolver(resolver);

            Assert.AreEqual(42, MetadataDeclaration.Get<int>());
        }

        [Test]
        public void Get_WithValidArguments_ReturnsMetadataResolverResults()
        {
            var maxLength = new MetadataDeclaration<int>(7);

            var module = new MetadataBindingModule();
            module.Bind(maxLength, 42);

            var resolver = new StandardMetadataResolver(module);

            MetadataDeclaration.SetResolver(resolver);

            var value = maxLength.For<string>();
            Assert.AreEqual(42, value);
        }

        [Test]
        public void Get_WithNoMetadataResolver_ReturnsDefault()
        {
            var maxLength = new MetadataDeclaration<int>(7);

            var value = maxLength.For<string>();
            Assert.AreEqual(7, value);
        }

        [Test]
        public void Get_WithNoMetadataResolverAndBadDefault_ThrowsValidationException()
        {
            var maxLengthWithBadDefault = new ComparableMetadataDeclaration<int>(1, 10, 42);

            Assert.Throws<MetadataValidationException<int>>(() => maxLengthWithBadDefault.For<string>());
        }

        [Test]
        public void Get_WithNoMetadataResolver_UsesResolverFromIoc()
        {
            var maxLength = new MetadataDeclaration<int>(7);

            var module = new MetadataBindingModule();
            module.Bind(maxLength, 42);

            var metadataResolver = new StandardMetadataResolver(module);

            Ioc.SetDependencyResolver(new DependencyResolver(decl => metadataResolver));

            var value = maxLength.For<string>();
            Assert.AreEqual(42, value);
        }

        [Test]
        public void Get_OnNonGenericMetadataWithDeclaration_ProvidesAllArgumentsToResolver()
        {
            var maxLength = new MetadataDeclaration<int>(7);

            var resolver = MockRepository.GenerateMock<IMetadataResolver>();
            resolver.Expect(x => x.Resolve<int, object>(maxLength, Maybe<object>.NoValue, null))
                .Return(42);

            MetadataDeclaration.SetResolver(resolver);

            maxLength.Get();
            resolver.VerifyAllExpectations();
        }

        [Test]
        public void Get_OnGenericMetadataWithDeclaration_ProvidesAllArgumentsToResolver()
        {
            var maxLength = new MetadataDeclaration<int>(7);

            var resolver = MockRepository.GenerateMock<IMetadataResolver>();
            resolver.Expect(x => x.Resolve<int, string>(maxLength, Maybe<string>.NoValue, null))
                .Return(42);

            MetadataDeclaration.SetResolver(resolver);

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

            MetadataDeclaration.SetResolver(resolver);

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
            resolver.Expect(x => x.Resolve<int, string>(maxLength, Maybe<string>.NoValue, member))
                .Return(42);

            MetadataDeclaration.SetResolver(resolver);

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

            MetadataDeclaration.SetResolver(resolver);

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

        [Test]
        public void Default_WhenExplicit_ReturnsCorrectly()
        {
            Assert.AreEqual(5, new ComparableMetadataDeclaration<int>(1, 10, 5).Default);
        }

        [Test]
        public void Default_WithExplicitInvalidDefault_ThrowsException()
        {
            var declaration = new ComparableMetadataDeclaration<int>(1, 10, 42);
            Assert.Throws<MetadataValidationException<int>>(() => { var x = declaration.Default; });
        }

        [Test]
        public void Default_WithImplicitInvalidDefault_ThrowsException()
        {
            var declaration = new ComparableMetadataDeclaration<int>(1, 10);
            Assert.Throws<MetadataValidationException<int>>(() => { var x = declaration.Default; });
        }

        [Test]
        public void Default_WhenImplicitForReferenceType_ReturnsCorrectly()
        {
            Assert.IsNull(new MetadataDeclaration<string>().Default);
        }

        [Test]
        public void Default_WhenImplicitForValueType_ReturnsCorrectly()
        {
            Assert.AreEqual(0, new MetadataDeclaration<int>().Default);
        }

        [Test]
        public void ValidateValue_WithValidValue_DoesNotThrowException()
        {
            var betweenOneAndTen = new ComparableMetadataDeclaration<int>(1, 10, 5);

            var module = new MetadataBindingModule();
            module.Bind(betweenOneAndTen, 7);

            var resolver = new StandardMetadataResolver(module);

            MetadataDeclaration.SetResolver(resolver);
            
            Assert.AreEqual(7, betweenOneAndTen.Get());
        }

        [Test]
        public void ValidateValue_WithoutValidValue_ThrowsException()
        {
            var betweenOneAndTen = new ComparableMetadataDeclaration<int>(1, 10, 5);

            var module = new MetadataBindingModule();
            module.Bind(betweenOneAndTen, 42);

            var resolver = new StandardMetadataResolver(module);

            MetadataDeclaration.SetResolver(resolver);

            Assert.Throws<MetadataValidationException<int>>(() => betweenOneAndTen.Get());
        }
    }
}
