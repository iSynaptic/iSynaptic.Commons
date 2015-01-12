// The MIT License
// 
// Copyright (c) 2012-2015 Jordan E. Terrell
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
using System.Linq;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;

namespace iSynaptic.Commons.Data
{
    [TestFixture]
    public class StandardExodataResolverTests
    {
        [SetUp]
        public void BeforeTest()
        {
            Ioc.SetDependencyResolver(null);
            BaseTestSubjectExodataSurrogate.ShouldYieldInstanceExodata = false;
        }

        [Test]
        public void Resolve_UsesIocToCreateSurrogates()
        {
            bool executed = false;

            Ioc.SetDependencyResolver(new DependencyResolver(x => x
                .ToMaybe()
                .OfType<IDependencySymbol>()
                .Where(y => typeof(BaseTestSubjectExodataSurrogate).IsAssignableFrom(y.DependencyType))
                .Select(y => Activator.CreateInstance(y.DependencyType))
                .OnValue(y => executed = true)));

            var resolver = new StandardExodataResolver();

            var value = resolver.TryResolve(StringExodata.MaxLength).For<TestSubject>(x => x.MiddleName);

            Assert.AreEqual(74088, value);
            Assert.IsTrue(executed);
        }

        [Test]
        public void Resolve_WithModuleProvidedMatchingBinding_ReturnsValue()
        {
            var resolver = new StandardExodataResolver(new TestExodataBindingModule());

            int value = resolver.TryResolve(StringExodata.MaxLength).Get();
            Assert.AreEqual(42, value);
        }

        [Test]
        public void TryResolve_AfterUnloadingModule_ReturnsNoValue()
        {
            var module = new TestExodataBindingModule();
            var resolver = new StandardExodataResolver(module);

            Maybe<int> value = resolver.TryResolve(StringExodata.MaxLength).TryGet();
            Assert.IsTrue(value == 42);

            resolver.UnloadModule(module);

            value = resolver.TryResolve(StringExodata.MaxLength).TryGet();
            Assert.IsTrue(value == Maybe<int>.NoValue);
        }

        [Test]
        public void Resolve_WithAttributedProperty_ReturnsValue()
        {
            var resolver = new StandardExodataResolver();

            var minLength = resolver.TryResolve(StringExodata.MinLength).For<TestSubject>(x => x.FirstName);
            Assert.AreEqual(21, minLength);

            var maxLength = resolver.TryResolve(StringExodata.MaxLength).For<TestSubject>(x => x.FirstName);
            Assert.AreEqual(84, maxLength);

            var description = resolver.TryResolve(CommonExodata.Description).For<TestSubject>(x => x.FirstName);
            Assert.AreEqual("First Name", description);
        }

        [Test]
        public void Resolve_WithAttributedField_ReturnsValue()
        {
            var resolver = new StandardExodataResolver();

            var allExodata = resolver.TryResolve(StringExodata.All).For<TestSubject>(x => x.LastName);

            Assert.AreEqual(7, allExodata.MinimumLength);
            Assert.AreEqual(1764, allExodata.MaximumLength);
            Assert.AreEqual("Last Name", allExodata.Description);
        }

        [Test]
        public void Resolve_WithAttributedFieldForBaseExodataClass_ReturnsValue()
        {
            var resolver = new StandardExodataResolver();

            var allExodata = resolver.TryResolve(CommonExodata.All).For<TestSubject>(x => x.LastName);

            Assert.IsNotNull(allExodata);
            Assert.AreEqual("Last Name", allExodata.Description);
        }

        [Test]
        public void Resolve_WithSurrogate_ReturnsValue()
        {
            var resolver = new StandardExodataResolver();

            var value = resolver.TryResolve(StringExodata.MaxLength).For<TestSubject>(x => x.MiddleName);
            Assert.AreEqual(74088, value);
        }

        [Test]
        public void Resolve_WithAttributedType_ReturnsValue()
        {
            var resolver = new StandardExodataResolver();

            var value = resolver.TryResolve(CommonExodata.Description).For<TestSubject>();
            Assert.AreEqual("Test Subject", value);
        }

        [Test]
        public void Resolve_WithModuleThatOverridesAttributeExodata_ReturnsValue()
        {
            var resolver = new StandardExodataResolver(new TestExodataBindingModule());

            var value = resolver.TryResolve(CommonExodata.Description).For<TestSubject>();
            Assert.AreEqual("Overridden Description", value);
        }

        [Test]
        public void Resolve_AgainstSubjectInstanceWithAttributedType_ReturnsValue()
        {
            var resolver = new StandardExodataResolver();

            var subject = new TestSubject();

            var value = resolver.TryResolve(CommonExodata.Description).For(subject);
            Assert.AreEqual("Test Subject", value);
        }

        [Test]
        public void Resolve_AgainstSpecificInstance_WorksCorrectly()
        {
            var resolver = new StandardExodataResolver();

            BaseTestSubjectExodataSurrogate.ShouldYieldInstanceExodata = true;

            var value = resolver.TryResolve(CommonExodata.Description).For(BaseTestSubjectExodataSurrogate.Subject);
            Assert.AreEqual("Special Instance Description", value);
        }

        [Test]
        public void Resolve_AgainstArbitraryInstance_YieldsAttributeExodata()
        {
            var resolver = new StandardExodataResolver();

            BaseTestSubjectExodataSurrogate.ShouldYieldInstanceExodata = false;

            var value = resolver.TryResolve(CommonExodata.Description).For(new TestSubject());
            Assert.AreEqual("Test Subject", value);
        }

        [Test]
        public void Resolve_AgainstArbitraryInstance_YieldsSurrogateExodata()
        {
            var resolver = new StandardExodataResolver();

            BaseTestSubjectExodataSurrogate.ShouldYieldInstanceExodata = true;

            var value = resolver.TryResolve(CommonExodata.Description).For(new TestSubject());
            Assert.AreEqual("Surrogate Description", value);
        }

        [Test]
        public void Resolve_AgainstSpecificInstanceWhenPredicateReturnsFalse_YieldsAttributeExodata()
        {
            var resolver = new StandardExodataResolver();

            BaseTestSubjectExodataSurrogate.ShouldYieldInstanceExodata = false;

            var value = resolver.TryResolve(CommonExodata.Description).For(BaseTestSubjectExodataSurrogate.Subject);
            Assert.AreEqual("Test Subject", value);
        }

        [Test]
        public void Resolve_AgainstArbitraryDerivedInstance_YieldsSurrogateExodata()
        {
            var resolver = new StandardExodataResolver();

            BaseTestSubjectExodataSurrogate.ShouldYieldInstanceExodata = true;

            var value = resolver.TryResolve(CommonExodata.Description).For(new DerivedTestSubject());
            Assert.AreEqual("Surrogate Description", value);
        }

        [Test]
        public void Resolve_WithDerivedInstance_YieldsMostDerivedBindingsExodata()
        {
            var resolver = new StandardExodataResolver();
            resolver.Bind(CommonExodata.Description)
                .For<DerivedTestSubject>()
                .To("Derived Surrogate Description");


            BaseTestSubjectExodataSurrogate.ShouldYieldInstanceExodata = true;

            var value = resolver.TryResolve(CommonExodata.Description).For(new DerivedTestSubject());
            Assert.AreEqual("Derived Surrogate Description", value);
        }

        [Test]
        public void Resolve_WithSpecificInstanceAgainstMember_YieldsExodataSurrogateMetadata()
        {
            var resolver = new StandardExodataResolver();

            BaseTestSubjectExodataSurrogate.ShouldYieldInstanceExodata = true;

            var value = resolver.TryResolve(CommonExodata.Description).For(BaseTestSubjectExodataSurrogate.Subject, x => x.FirstName);
            Assert.AreEqual("Special Member Description", value);
        }

        [Test]
        public void Resolve_WithSimpleStaticBinding_YieldsExodataSurrogateExodata()
        {
            var resolver = new StandardExodataResolver();

            Assert.AreEqual("A string...", resolver.TryResolve(CommonExodata.Description).For<string>());
        }

        [Test]
        public void Resolve_WithContext_YieldsContextualExodataSurrogateExodata()
        {
            var resolver = new StandardExodataResolver();

            BaseTestSubjectExodataSurrogate.ShouldYieldInstanceExodata = true;

            Assert.AreEqual("Contextual Member Description", resolver.TryResolve(CommonExodata.Description).Given<string>().For<TestSubject>(x => x.FirstName));
        }

        [Test]
        public void Resolve_WithSpecificContext_YieldsSpecificContextualExodataSurrogateExodata()
        {
            var resolver = new StandardExodataResolver();

            BaseTestSubjectExodataSurrogate.ShouldYieldInstanceExodata = true;

            Assert.AreEqual("Specific Contextual Member Description", resolver.TryResolve(CommonExodata.Description).Given("Context").For<TestSubject>(x => x.FirstName));
        }

        [Test]
        public void Resolve_WithMultipleMembers_YieldsExodata()
        {
            var resolver = new StandardExodataResolver();
            resolver.Bind(IntegerExodata.MinValue)
                .For<DateTime>(x => x.Day, x => x.Month)
                .To(42);

            Assert.AreEqual(42, resolver.TryResolve(IntegerExodata.MinValue).For<DateTime>(x => x.Day));
            Assert.AreEqual(42, resolver.TryResolve(IntegerExodata.MinValue).For<DateTime>(x => x.Month));
        }
    }
}
