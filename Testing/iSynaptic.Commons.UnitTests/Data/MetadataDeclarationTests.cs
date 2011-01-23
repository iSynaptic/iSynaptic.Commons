using System;
using System.Collections.Generic;
using System.Linq;
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

            var resolver = MockRepository.GenerateStub<IMetadataResolver>();
            resolver.Stub(x => x.Resolve(betweenOneAndTen, Maybe<object>.NoValue, null))
                .IgnoreArguments()
                .Return(7);

            Metadata.SetResolver(resolver);
            
            Assert.AreEqual(7, betweenOneAndTen.Get());
        }

        [Test]
        public void ValidateValue_WithoutValidValue_ThrowsException()
        {
            var betweenOneAndTen = new ComparableMetadataDeclaration<int>(1, 10, 5);

            var resolver = MockRepository.GenerateStub<IMetadataResolver>();
            resolver.Stub(x => x.Resolve(betweenOneAndTen, Maybe<object>.NoValue, null))
                .IgnoreArguments()
                .Return(42);

            Metadata.SetResolver(resolver);

            Assert.Throws<MetadataValidationException<int>>(() => betweenOneAndTen.Get());
        }
    }
}
