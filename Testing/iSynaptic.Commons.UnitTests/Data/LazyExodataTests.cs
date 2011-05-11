using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;

using iSynaptic.Commons.Data.Syntax;

namespace iSynaptic.Commons.Data
{
    [TestFixture]
    public class LazyExodataTests
    {
        [SetUp]
        public void BeforeTest()
        {
            ExodataDeclaration.SetResolver(null);
            Ioc.SetDependencyResolver(null);
        }

        [Test]
        public void LazyExodata_ViaDeclaration()
        {
            var resolver = new StandardExodataResolver();
            resolver.Bind(StringExodata.MaxLength, 42);

            ExodataDeclaration.SetResolver(resolver);

            Assert.AreEqual(42, StringExodata.MaxLength.LazyGet());
        }

        [Test]
        public void LazyExodata_ViaDeclarationSubjectType()
        {
            var resolver = new StandardExodataResolver();
            resolver.Bind(StringExodata.MaxLength)
                .For<string>()
                .To(42);

            ExodataDeclaration.SetResolver(resolver);

            Assert.AreEqual(42, StringExodata.MaxLength.LazyFor<string>());
        }

        [Test]
        public void LazyExodata_ViaDeclarationSubject()
        {
            string subject = "Hello, World!";

            var resolver = new StandardExodataResolver();
            resolver.Bind(StringExodata.MaxLength)
                .For(subject)
                .To(42);

            ExodataDeclaration.SetResolver(resolver);

            Assert.AreEqual(42, StringExodata.MaxLength.LazyFor(subject));
        }

        [Test]
        public void LazyExodata_ViaDeclarationMember()
        {
            var resolver = new StandardExodataResolver();
            resolver.Bind(IntegerExodata.MinValue)
                .For<string>(x => x.Length)
                .To(42);

            ExodataDeclaration.SetResolver(resolver);

            Assert.AreEqual(42, IntegerExodata.MinValue.LazyFor<string>(x => x.Length));
        }

        [Test]
        public void LazyExodata_ViaDeclarationSubjectMember()
        {
            string subject = "Hello, World!";
            Expression<Func<string, object>> expression = x => x.Length;

            var resolver = new StandardExodataResolver();
            resolver.Bind(IntegerExodata.MinValue)
                .For(subject, expression)
                .To(42);

            ExodataDeclaration.SetResolver(resolver);

            Assert.AreEqual(42, IntegerExodata.MinValue.LazyFor(subject, expression));
        }
    }
}
