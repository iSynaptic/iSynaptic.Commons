// The MIT License
// 
// Copyright (c) 2012 Jordan E. Terrell
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
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
        public void Get_WhenWithNoDefault_ThrowsException()
        {
            var maxLength = new ExodataDeclaration<int>();
            Assert.Throws<InvalidOperationException>(() => maxLength.Get());
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
        public void Get_WithNoExodataResolverAndBadDefault_ThrowsValidationExceptionWithCorrectInvalidValue()
        {
            var maxLengthWithBadDefault = new ComparableExodataDeclaration<int>(1, 10, 42);

            var exception = Assert.Throws<ExodataValidationException<int>>(() => maxLengthWithBadDefault.For<string>());
            Assert.AreEqual(42, exception.InvalidValue);
        }

        [Test]
        public void Get_WithNoExodataResolver_UsesResolverFromIoc()
        {
            var maxLength = new ExodataDeclaration<int>(7);

            var exodataResolver = new StandardExodataResolver();
            exodataResolver.Bind(maxLength)
                .For<string>()
                .To(42);

            Ioc.SetDependencyResolver(new DependencyResolver(symbol => symbol.ToMaybe().OfType<IDependencySymbol>().Where(x => x.DependencyType == typeof(IExodataResolver)).Select(x => (object)exodataResolver)));

            var value = maxLength.For<string>();
            Assert.AreEqual(42, value);
        }

        [Test]
        public void Get_OnNonGenericExodataWithDeclaration_ProvidesAllArgumentsToResolver()
        {
            var maxLength = new ExodataDeclaration<int>(7);

            var resolver = MockRepository.GenerateMock<IExodataResolver>();
            resolver.Expect(x => x.TryResolve<int, object, object>(null))
                .Callback<IExodataRequest<int, object, object>>(r => r.Symbol == maxLength &&
                                                        r.Context == Maybe<object>.NoValue &&
                                                        r.Subject == Maybe<object>.NoValue &&
                                                        r.Member == null)
                .Return(42.ToMaybe());

            ExodataDeclaration.SetResolver(resolver);

            maxLength.Get();
            resolver.VerifyAllExpectations();
        }

        [Test]
        public void Get_OnGenericExodataWithDeclaration_ProvidesAllArgumentsToResolver()
        {
            var maxLength = new ExodataDeclaration<int>(7);

            var resolver = MockRepository.GenerateMock<IExodataResolver>();
            resolver.Expect(x => x.TryResolve<int, object, string>(null))
                .Callback<IExodataRequest<int, object, string>>(r => r.Symbol == maxLength &&
                                                                     r.Context == Maybe<object>.NoValue &&
                                                                     r.Subject == Maybe<string>.NoValue &&
                                                                     r.Member == null)
                .Return(42.ToMaybe());

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
            resolver.Expect(x => x.TryResolve<int, object, string>(null))
                .Callback<IExodataRequest<int, object, string>>(r => r.Symbol == maxLength &&
                                                                     r.Context == Maybe<object>.NoValue &&
                                                                     r.Subject == subject &&
                                                                     r.Member == null)
                .Return(42.ToMaybe());

            ExodataDeclaration.SetResolver(resolver);

            maxLength.For(subject);
            resolver.VerifyAllExpectations();
        }

        [Test]
        public void Get_OnGenericExodataWithDeclarationAndMember_ProvidesAllArgumentsToResolver()
        {
            Expression<Func<string, object>> expression = x => x.Length;
            var member = expression.ExtractMemberInfoForExodata<string>();

            var maxLength = new ExodataDeclaration<int>(7);

            var resolver = MockRepository.GenerateMock<IExodataResolver>();
            resolver.Expect(x => x.TryResolve<int, object, string>(null))
                .Callback<IExodataRequest<int, object, string>>(r => r.Symbol == maxLength &&
                                                                     r.Context == Maybe<object>.NoValue &&
                                                                     r.Subject == Maybe<string>.NoValue &&
                                                                     r.Member == member)
                .Return(42.ToMaybe());

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
            var member = expression.ExtractMemberInfoForExodata<string>();

            var resolver = MockRepository.GenerateMock<IExodataResolver>();
            resolver.Expect(x => x.TryResolve<int, object, string>(null))
                .Callback<IExodataRequest<int, object, string>>(r => r.Symbol == maxLength &&
                                                        r.Context == Maybe<object>.NoValue &&
                                                        r.Subject == subject &&
                                                        r.Member == member)
                .Return(42.ToMaybe());

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
