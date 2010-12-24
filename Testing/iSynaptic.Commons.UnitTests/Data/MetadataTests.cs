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
    public class MetadataTests
    {
        [SetUp]
        public void BeforeTest()
        {
            Metadata.SetMetadataResolver(null);
            Ioc.SetDependencyResolver(null);
        }

        [Test]
        public void Get_WithValidArguments_ReturnsMetadataResolverResults()
        {
            var maxLength = new MetadataDeclaration<int>(7);

            var resolver = MockRepository.GenerateStub<IMetadataResolver>();
            resolver.Stub(x => x.Resolve(maxLength, typeof (string), null))
                .Return(42);

            Metadata.SetMetadataResolver(resolver);

            var value = Metadata<string>.Get(maxLength);
            Assert.AreEqual(42, value);
        }

        [Test]
        public void Get_WithNullDeclaration_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => 
                Metadata<string>.Get<int>(null));
        }

        [Test]
        public void Get_WithNoMetadataResolver_ReturnsDefault()
        {
            var maxLength = new MetadataDeclaration<int>(7);

            var value = Metadata<string>.Get(maxLength);
            Assert.AreEqual(7, value);
        }

        [Test]
        public void Get_WithNoMetadataResolverAndBadDefault_ThrowsWrappedException()
        {
            var maxLengthWithBadDefault = new IntegerMetadataDeclaration(1, 10, 42);

            Assert.That(() => { Metadata<string>.Get(maxLengthWithBadDefault); },
                Throws
                    .InstanceOf<InvalidOperationException>().And
                    .InnerException
                        .InstanceOf<InvalidOperationException>().And
                    .InnerException.InnerException
                                .InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void Get_WithNoMetadataResolver_UsesResolverFromIoc()
        {
            var maxLength = new MetadataDeclaration<int>(7);

            var metadataResolver = MockRepository.GenerateStub<IMetadataResolver>();
            metadataResolver.Stub(x => x.Resolve(maxLength, typeof(string), null))
                .Return(42);

            Ioc.SetDependencyResolver(new DependencyResolver((k, d, r) => metadataResolver));

            var value = Metadata<string>.Get(maxLength);
            Assert.AreEqual(42, value);
        }

        [Test]
        public void Get_OnNonGenericMetadataWithDeclaration_ProvidesAllArgumentsToResolver()
        {
            var maxLength = new MetadataDeclaration<int>(7);

            var resolver = MockRepository.GenerateMock<IMetadataResolver>();
            resolver.Expect(x => x.Resolve(maxLength, null, null))
                .Return(42);

            Metadata.SetMetadataResolver(resolver);

            Metadata.Get(maxLength);
            resolver.VerifyAllExpectations();
        }

        [Test]
        public void Get_OnGenericMetadataWithDeclaration_ProvidesAllArgumentsToResolver()
        {
            var maxLength = new MetadataDeclaration<int>(7);

            var resolver = MockRepository.GenerateMock<IMetadataResolver>();
            resolver.Expect(x => x.Resolve(maxLength, typeof(string), null))
                .Return(42);

            Metadata.SetMetadataResolver(resolver);

            Metadata<string>.Get(maxLength);
            resolver.VerifyAllExpectations();
        }

        [Test]
        public void Get_OnGenericMetadataWithDeclarationAndSubject_ProvidesAllArgumentsToResolver()
        {
            var maxLength = new MetadataDeclaration<int>(7);
            string subject = "Hello, World!";

            var resolver = MockRepository.GenerateMock<IMetadataResolver>();
            resolver.Expect(x => x.Resolve(maxLength, subject, null))
                .Return(42);

            Metadata.SetMetadataResolver(resolver);

            Metadata<string>.Get(maxLength, subject);
            resolver.VerifyAllExpectations();
        }

        [Test]
        public void Get_OnGenericMetadataWithDeclarationAndMember_ProvidesAllArgumentsToResolver()
        {
            var maxLength = new MetadataDeclaration<int>(7);

            Expression<Func<string, int>> expression = x => x.Length;
            var member = ((MemberExpression) expression.Body).Member;

            var resolver = MockRepository.GenerateMock<IMetadataResolver>();
            resolver.Expect(x => x.Resolve(maxLength, typeof(string), member))
                .Return(42);

            Metadata.SetMetadataResolver(resolver);

            Metadata<string>.Get(maxLength, expression);
            resolver.VerifyAllExpectations();
        }

        [Test]
        public void Get_OnGenericMetadataWithDeclarationSubjectAndMember_ProvidesAllArgumentsToResolver()
        {
            var maxLength = new MetadataDeclaration<int>(7);
            string subject = "Hello, World!";

            Expression<Func<string, int>> expression = x => x.Length;
            var member = ((MemberExpression)expression.Body).Member;

            var resolver = MockRepository.GenerateMock<IMetadataResolver>();
            resolver.Expect(x => x.Resolve(maxLength, subject, member))
                .Return(42);

            Metadata.SetMetadataResolver(resolver);

            Metadata<string>.Get(maxLength, subject, expression);
            resolver.VerifyAllExpectations();
        }

        [Test]
        public void Construction_OfObjectThatInheritsFromMetadata_NotSupported()
        {
            Assert.Throws<NotSupportedException>(() => new ClassThanInheritsFromMetadata());
        }

        public class ClassThanInheritsFromMetadata : Metadata<string>
        {
        }
    }
}
