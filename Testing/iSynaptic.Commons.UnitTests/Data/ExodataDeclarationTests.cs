using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using iSynaptic.Commons.Data.ExodataDeclarations;
using NUnit.Framework;
using Rhino.Mocks;

namespace iSynaptic.Commons.Data
{
    [TestFixture]
    public class ExodataDeclarationTests
    {
        [SetUp]
        public void BeforeTest()
        {
            ExodataDeclaration.SetResolver(null);
            Ioc.SetDependencyResolver(null);
        }

        [Test]
        public void Resolve_ThruImplicitCastOperator_ReturnsValue()
        {
            var resolver = new StandardExodataResolver();
            resolver.Bind(StringExodata.MaxLength, 42);

            ExodataDeclaration.SetResolver(resolver);

            int value = StringExodata.MaxLength;
            Assert.AreEqual(42, value);
        }

        [Test]
        public void Get_ByTypeOnly_UsesTypeDeclaration()
        {
            var resolver = new StandardExodataResolver();
            resolver.Bind(ExodataDeclaration<int>.TypeDeclaration, 42);

            ExodataDeclaration.SetResolver(resolver);

            Assert.AreEqual(42, ExodataDeclaration.Get<int>());
        }

        [Test]
        public void Get_WithValidArguments_ReturnsExodataResolverResults()
        {
            var maxLength = new ExodataDeclaration<int>(7);

            var resolver = new StandardExodataResolver();
            resolver.Bind(maxLength)
                .For<string>()
                .To(42);

            ExodataDeclaration.SetResolver(resolver);

            var value = maxLength.For<string>();
            Assert.AreEqual(42, value);
        }

        [Test]
        public void Get_WithNoExodataResolver_ReturnsDefault()
        {
            var maxLength = new ExodataDeclaration<int>(7);

            var value = maxLength.For<string>();
            Assert.AreEqual(7, value);
        }

        [Test]
        public void Get_WithNoExodataResolverAndBadDefault_ThrowsValidationException()
        {
            var maxLengthWithBadDefault = new ComparableExodataDeclaration<int>(1, 10, 42);

            Assert.Throws<ExodataValidationException<int>>(() => maxLengthWithBadDefault.For<string>());
        }

        [Test]
        public void Get_WithNoExodataResolver_UsesResolverFromIoc()
        {
            var maxLength = new ExodataDeclaration<int>(7);

            var exodataResolver = new StandardExodataResolver();
            exodataResolver.Bind(maxLength)
                .For<string>()
                .To(42);

            Ioc.SetDependencyResolver(new DependencyResolver(decl => decl.DependencyType == typeof(IExodataResolver) ? exodataResolver : null));

            var value = maxLength.For<string>();
            Assert.AreEqual(42, value);
        }

        [Test]
        public void Get_OnNonGenericExodataWithDeclaration_ProvidesAllArgumentsToResolver()
        {
            var maxLength = new ExodataDeclaration<int>(7);

            var resolver = MockRepository.GenerateMock<IExodataResolver>();
            resolver.Expect(x => x.Resolve<int, object>(new ExodataRequest<object>(maxLength, Maybe<object>.NoValue, null)))
                .Return(42);

            ExodataDeclaration.SetResolver(resolver);

            maxLength.Get();
            resolver.VerifyAllExpectations();
        }

        [Test]
        public void Get_OnGenericExodataWithDeclaration_ProvidesAllArgumentsToResolver()
        {
            var maxLength = new ExodataDeclaration<int>(7);

            var resolver = MockRepository.GenerateMock<IExodataResolver>();
            resolver.Expect(x => x.Resolve<int, string>(new ExodataRequest<string>(maxLength, Maybe<string>.NoValue, null)))
                .Return(42);

            ExodataDeclaration.SetResolver(resolver);

            maxLength.For<string>();
            resolver.VerifyAllExpectations();
        }

        [Test]
        public void Get_OnGenericExodataWithDeclarationAndSubject_ProvidesAllArgumentsToResolver()
        {
            var maxLength = new ExodataDeclaration<int>(7);
            string subject = "Hello, World!";

            var resolver = MockRepository.GenerateMock<IExodataResolver>();
            resolver.Expect(x => x.Resolve<int, string>(new ExodataRequest<string>(maxLength, new Maybe<string>(subject), null)))
                .Return(42);

            ExodataDeclaration.SetResolver(resolver);

            maxLength.For(subject);
            resolver.VerifyAllExpectations();
        }

        [Test]
        public void Get_OnGenericExodataWithDeclarationAndMember_ProvidesAllArgumentsToResolver()
        {
            Expression<Func<string, object>> expression = x => x.Length;
            var member = expression.ExtractMemberInfoForExodata();

            var maxLength = new ExodataDeclaration<int>(7);

            var resolver = MockRepository.GenerateMock<IExodataResolver>();
            resolver.Expect(x => x.Resolve<int, string>(new ExodataRequest<string>(maxLength, Maybe<string>.NoValue, member)))
                .Return(42);

            ExodataDeclaration.SetResolver(resolver);

            maxLength.For<string>(expression);
            resolver.VerifyAllExpectations();
        }

        [Test]
        public void Get_OnGenericExodataWithDeclarationSubjectAndMember_ProvidesAllArgumentsToResolver()
        {
            var maxLength = new ExodataDeclaration<int>(7);
            string subject = "Hello, World!";

            Expression<Func<string, object>> expression = x => x.Length;
            var member = expression.ExtractMemberInfoForExodata();

            var resolver = MockRepository.GenerateMock<IExodataResolver>();
            resolver.Expect(x => x.Resolve<int, string>(new ExodataRequest<string>(maxLength, new Maybe<string>(subject), member)))
                .Return(42);

            ExodataDeclaration.SetResolver(resolver);

            maxLength.For(subject, expression);
            resolver.VerifyAllExpectations();
        }

        [Test]
        public void Get_OnNestedProperty_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
                StringExodata.MaxLength
                    .For<TestSubject>(x => x.MiddleName.Length));
        }

        [Test]
        public void Get_OnMethodMember_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
                StringExodata.MinLength
                    .For<TestSubject>(x => x.ToString()));
        }

        [Test]
        public void Get_OnMethodNestedMember_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
                StringExodata.MinLength
                    .For<TestSubject>(x => x.ToString().Length));
        }

        [Test]
        public void Default_WhenExplicit_ReturnsCorrectly()
        {
            Assert.AreEqual(5, new ComparableExodataDeclaration<int>(1, 10, 5).Default);
        }

        [Test]
        public void Default_WithExplicitInvalidDefault_ThrowsException()
        {
            var declaration = new ComparableExodataDeclaration<int>(1, 10, 42);
            Assert.Throws<ExodataValidationException<int>>(() => { var x = declaration.Default; });
        }

        [Test]
        public void Default_WithImplicitInvalidDefault_ThrowsException()
        {
            var declaration = new ComparableExodataDeclaration<int>(1, 10);
            Assert.Throws<ExodataValidationException<int>>(() => { var x = declaration.Default; });
        }

        [Test]
        public void Default_WhenImplicitForReferenceType_ReturnsCorrectly()
        {
            Assert.IsNull(new ExodataDeclaration<string>().Default);
        }

        [Test]
        public void Default_WhenImplicitForValueType_ReturnsCorrectly()
        {
            Assert.AreEqual(0, new ExodataDeclaration<int>().Default);
        }

        [Test]
        public void ValidateValue_WithValidValue_DoesNotThrowException()
        {
            var betweenOneAndTen = new ComparableExodataDeclaration<int>(1, 10, 5);

            var resolver = new StandardExodataResolver();
            resolver.Bind(betweenOneAndTen, 7);

            ExodataDeclaration.SetResolver(resolver);
            
            Assert.AreEqual(7, betweenOneAndTen.Get());
        }

        [Test]
        public void ValidateValue_WithoutValidValue_ThrowsException()
        {
            var betweenOneAndTen = new ComparableExodataDeclaration<int>(1, 10, 5);

            var resolver = new StandardExodataResolver();
            resolver.Bind(betweenOneAndTen, 42);

            ExodataDeclaration.SetResolver(resolver);

            Assert.Throws<ExodataValidationException<int>>(() => betweenOneAndTen.Get());
        }
    }
}
