using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace iSynaptic.Commons
{
    [TestFixture]
    public class GuardTests
    {
        [Test]
        public void MustBeDefined_WithUndefinedValue_ThrowsException()
        {
            var dayOfWeek = (DayOfWeek) int.MaxValue - 1;

            Assert.Throws<ArgumentOutOfRangeException>(() => Guard.MustBeDefined(dayOfWeek, "value"));
        }

        [Test]
        public void MustBeDefined_WithDefinedValue_DoesNothing()
        {
            Guard.MustBeDefined(DayOfWeek.Friday, "value");
        }

        [Test]
        public void NotNull_WithNullValue_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => Guard.NotNull<object>(null, "value"));
        }

        [Test]
        public void NotNull_WithNonNullValue_DoesNothing()
        {
            Guard.NotNull(new object(), "value");
        }

        [Test]
        public void NotNullOrEmpty_WithNullString_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => Guard.NotNullOrEmpty(null, "value"));
        }

        [Test]
        public void NotNullOrEmpty_WithEmptyString_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => Guard.NotNullOrEmpty("", "value"));
        }

        [Test]
        public void NotNullOrEmpty_WithNonNullOrEmptyString_DoesNothing()
        {
            Guard.NotNullOrEmpty("Hello, World!", "value");
        }

        [Test]
        public void NotNullOrWhiteSpace_WithNullString_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => Guard.NotNullOrWhiteSpace(null, "value"));
        }

        [Test]
        public void NotNullOrWhiteSpace_WithEmptyString_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => Guard.NotNullOrWhiteSpace("", "value"));
        }

        [Test]
        public void NotNullOrWhiteSpace_WithWhiteSpaceString_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => Guard.NotNullOrWhiteSpace(" ", "value"));
        }

        [Test]
        public void NotNullOrWhiteSpace_WithNonNullOrWhiteSpaceString_DoesNothing()
        {
            Guard.NotNullOrWhiteSpace("Hello, World!", "value");
        }

        [Test]
        public void NotWhiteSpace_WithNullString_DoesNothing()
        {
            Guard.NotWhiteSpace(null, "value");
        }

        [Test]
        public void NotEmpty_WithEmpyGuid_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => Guard.NotEmpty(Guid.Empty, "value"));
        }

        [Test]
        public void NotEmpty_WithNonEmptyGuid_DoesNothing()
        {
            Guard.NotEmpty(Guid.NewGuid(), "value");
        }

        [Test]
        public void NotEmpty_WithEmptyEnumerable_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => Guard.NotEmpty(Enumerable.Empty<int>(), "value"));
        }

        [Test]
        public void NotEmpty_WithNonEmptyEnumerable_DoesNothing()
        {
            Guard.NotEmpty(Enumerable.Range(1, 1), "value");
        }

        [Test]
        public void NotNullOrEmpty_WithNullEnumerable_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => Guard.NotNullOrEmpty((IEnumerable<int>) null, "value"));
        }

        [Test]
        public void NotNullOrEmpty_WithEmptyEnumerable_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => Guard.NotNullOrEmpty(Enumerable.Empty<int>(), "value"));
        }

        [Test]
        public void NotNullOrEmpty_WithNonNullOrEmptyEnumerable_DoesNothing()
        {
            Guard.NotNullOrEmpty(Enumerable.Range(1,1), "value");
        }
    }
}
