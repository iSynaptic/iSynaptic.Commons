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

        [Test]
        public void GetFieldsDeeply_WorksRecursively()
        {
            var fields = typeof (FurtherDerived).GetFieldsDeeply()
                .Select(x => x.Name)
                .OrderBy(x => x);

            Assert.IsTrue(fields.SequenceEqual(new []{"Bar", "Baz", "Foo", "Quux"}));
        }

        [Test]
        public void GetFieldsDeeply_WithFilter_WorksRecursively()
        {
            var fields = typeof(FurtherDerived).GetFieldsDeeply(x => !x.Name.StartsWith("B"))
                .Select(x => x.Name)
                .OrderBy(x => x);

            Assert.IsTrue(fields.SequenceEqual(new[] { "Foo", "Quux" }));
        }

        private class Base
        {
            public Guid Quux = Guid.Empty;
        }

        private class Derived : Base
        {
            protected int Baz = 42;
            public DateTime Bar = SystemClock.Now;
       }

        private class FurtherDerived : Derived
        {
            private string Foo = "Foo";

            public FurtherDerived()
            {
                Console.WriteLine(Foo);
            }
        }
    }
}
