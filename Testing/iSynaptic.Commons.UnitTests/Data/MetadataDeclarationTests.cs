using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iSynaptic.Commons.Data.MetadataDeclarations;
using NUnit.Framework;

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

            for (int i = betweenOneAndTen.MinValue; i <= betweenOneAndTen.MaxValue; i++)
                betweenOneAndTen.ValidateValue(i);
        }

        [Test]
        public void ValidateValue_WithoutValidValue_ThrowsException()
        {
            var betweenOneAndTen = new ComparableMetadataDeclaration<int>(1, 10, 5);

            Assert.Throws<MetadataValidationException<int>>(() => betweenOneAndTen.ValidateValue(0));
        }
    }
}
