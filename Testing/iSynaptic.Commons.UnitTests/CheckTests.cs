using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace iSynaptic.Commons
{
    [TestFixture]
    public class CheckTests
    {
        [Test]
        public void NotNull_WithNullValue_ReturnsFailure()
        {
            string nullValue = null;

            var outcome = Check.NotNull(nullValue, "nullValue");
            Assert.IsFalse(outcome.WasSuccessful);

            var observations = outcome.Observations.ToList();

            Assert.AreEqual(1, observations.Count);
            Assert.AreEqual(CheckType.NotNull, observations[0].Type);
            Assert.AreEqual("nullValue", observations[0].Name);
        }

        [Test]
        public void Check_AndingSyntax_IsSupported()
        {
            string notNullValue = "Hello, World!";
            string nullValue = null;

            var outcome = Check.NotNull(notNullValue, "notNullValue") &
                          Check.NotNull(nullValue, "nullValue");

            Assert.IsFalse(outcome.WasSuccessful);

            var observations = outcome.Observations.ToList();

            Assert.AreEqual(1, observations.Count);
            Assert.AreEqual(CheckType.NotNull, observations[0].Type);
            Assert.AreEqual("nullValue", observations[0].Name);
        }
    }
}
