using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace iSynaptic.Commons.Reflection
{
    [TestFixture]
    public class ReflectionExtensionsTests
    {
        [Test]
        public void GetAttributesOfType_WithNullProvider_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(
                () => ReflectionExtensions.GetAttributesOfType<TestFixtureAttribute>(null));
        }

        [Test]
        public void GetAttributesOfType_AgainstThisTestFixture_ReturnsTestFixtureAttribute()
        {
            var attribute = GetType()
                .GetAttributesOfType<TestFixtureAttribute>()
                .Single();

            Assert.IsNotNull(attribute);
        }

        [Test]
        public void GetFieldsDeeply_WithNullSource_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ReflectionExtensions.GetFieldsDeeply(null));
        }
    }
}
