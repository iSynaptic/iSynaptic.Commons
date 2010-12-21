using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace iSynaptic.Commons.Data
{
    [TestFixture]
    public class MetadataDeclarationTests
    {
        [Test]
        public void Default_WhenExplicit_ReturnsCorrectly()
        {
            Assert.AreEqual(5, new IntegerMetadataDeclaration(1, 10, 5).Default);
        }

        [Test]
        public void Default_WithExplicitInvalidDefault_ThrowsException()
        {
            var declaration = new IntegerMetadataDeclaration(1, 10, 42);
            Assert.That(() => { var x = declaration.Default; },
                Throws
                .InstanceOf<InvalidOperationException>().And
                .InnerException
                    .InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void Default_WithImplicitInvalidDefault_ThrowsException()
        {
            var declaration = new IntegerMetadataDeclaration(1, 10);
            Assert.That(() => { var x = declaration.Default; },
                Throws
                .InstanceOf<InvalidOperationException>().And
                .InnerException
                    .InstanceOf<ArgumentOutOfRangeException>());
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
        public void CheckValue_WithValidValue_DoesNotThrowException()
        {
            var betweenOneAndTen = new IntegerMetadataDeclaration(1, 10, 5);

            for (int i = betweenOneAndTen.Min; i <= betweenOneAndTen.Max; i++)
                betweenOneAndTen.CheckValue(i);
        }

        [Test]
        public void CheckValue_WithoutValidValue_ThrowsException()
        {
            var betweenOneAndTen = new IntegerMetadataDeclaration(1, 10, 5);

            Assert.Throws<ArgumentOutOfRangeException>(() => betweenOneAndTen.CheckValue(0));
        }
    }
}
