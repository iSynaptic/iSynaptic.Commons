// The MIT License
// 
// Copyright (c) 2011 Jordan E. Terrell
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

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

            Assert.Throws<ArgumentOutOfRangeException>(() => Guard.MustBeDefined<DayOfWeek>(dayOfWeek, "value"));
        }

        [Test]
        public void MustBeDefined_CannotBeUsedWithNonEnumTypeArgument()
        {
            Assert.Throws<ArgumentException>(() => Guard.MustBeDefined<int>(DayOfWeek.Wednesday, "value"));
        }

        [Test]
        public void MustBeDefined_WithDefinedValue_DoesNothing()
        {
            Guard.MustBeDefined<DayOfWeek>(DayOfWeek.Friday, "value");
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

        [Test]
        public void MustBeGreaterThan_WhenValueLessThanOrEqualToComparisionValue_ThrowsException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Guard.MustBeGreaterThan(42, 42, "value"));
            Assert.Throws<ArgumentOutOfRangeException>(() => Guard.MustBeGreaterThan(42, 43, "value"));
        }

        [Test]
        public void MustBeGreaterThan_WhenValueIsGreater_ReturnsValue()
        {
            Assert.AreEqual(42, Guard.MustBeGreaterThan(42, 0, "value"));
        }

        [Test]
        public void MustBeGreaterThanOrEqual_WhenValueLessThanComparisionValue_ThrowsException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Guard.MustBeGreaterThanOrEqual(42, 43, "value"));
        }

        [Test]
        public void MustBeGreaterThanOrEqual_WhenValueIsGreaterOrEqual_ReturnsValue()
        {
            Assert.AreEqual(42, Guard.MustBeGreaterThanOrEqual(42, 41, "value"));
            Assert.AreEqual(42, Guard.MustBeGreaterThanOrEqual(42, 42, "value"));
        }

        [Test]
        public void MustBeLessThan_WhenValueGreaterThanOrEqualToComparisionValue_ThrowsException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Guard.MustBeLessThan(42, 42, "value"));
            Assert.Throws<ArgumentOutOfRangeException>(() => Guard.MustBeLessThan(42, 41, "value"));
        }

        [Test]
        public void MustBeLessThan_WhenValueIsLessThan_ReturnsValue()
        {
            Assert.AreEqual(42, Guard.MustBeLessThan(42, 43, "value"));
        }

        [Test]
        public void MustBeLessThanOrEqual_WhenValueGreaterThanComparisionValue_ThrowsException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Guard.MustBeLessThanOrEqual(42, 41, "value"));
        }

        [Test]
        public void MustBeLessThanOrEqual_WhenValueIsLessOrEqual_ReturnsValue()
        {
            Assert.AreEqual(42, Guard.MustBeLessThanOrEqual(42, 43, "value"));
            Assert.AreEqual(42, Guard.MustBeLessThanOrEqual(42, 42, "value"));
        }

        [Test]
        public void Matches_WithMatch_DoesNothing()
        {
            Assert.AreEqual("Hello", Guard.Matches("Hello", @"^H\w+$", "value"));
        }

        [Test]
        public void Matches_WithoutMatch_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => Guard.Matches("Bye", @"^H\w+$", "value"));
        }
    }
}
