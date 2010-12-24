using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace iSynaptic.Commons
{
    [TestFixture]
    public class CastTests
    {
        [Test]
        public void Cast_ValueType_ReturnsCorrectly()
        {
            Assert.AreEqual(42, CastToInt(42));
        }

        [Test]
        public void Cast_ReferenceType_ReturnsCorrectly()
        {
            Assert.AreEqual("Hello, World!", CastToString("Hello, World!"));

        }

        private int CastToInt<T>(T source)
        {
            return Cast.To<T, int>(source);
        }

        private string CastToString<T>(T source)
        {
            return Cast.To<T, string>(source);
        }
    }
}
