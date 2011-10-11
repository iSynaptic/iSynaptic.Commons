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
    public class ResultTests
    {
        [Test]
        public void AccessingValueProperty_OnNoValue_ThrowsInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() => Console.WriteLine(Result<string, string>.NoValue.Value));
        }

        [Test]
        public void HasValueProperty_OnNoValue_ReturnsFalse()
        {
            Assert.IsFalse(Result<string, string>.NoValue.HasValue);
        }

        [Test]
        public void HasValueProperty_OnValue_ReturnsTrue()
        {
            Assert.IsTrue(new Result<string, string>("").HasValue);
        }

        [Test]
        public void AccessingValueProperty_OnValue_ReturnsExpectedValue()
        {
            Assert.IsNull(new Result<string, int>((string)null).Value);
            Assert.AreEqual("Hello, World!", new Result<string, int>("Hello, World!").Value);
        }

        [Test]
        public void Observations_WithNoValue_IsEmpty()
        {
            Assert.IsTrue(Result<int, string>.NoValue.Observations.Count() == 0);
        }

        [Test]
        public void Observations_WithDefaultValue_IsEmpty()
        {
            Assert.IsTrue(Result<int, string>.Default.Observations.Count() == 0);
        }

        [Test]
        public void ExplicitCast_OnValue_ReturnsValue()
        {
            int value = (int)new Result<int, string>(42);

            Assert.AreEqual(42, value);
        }

        [Test]
        public void ExplicitCast_OnNoValue_ThrowsException()
        {
            Assert.Throws<InvalidOperationException>(() => { var x = (int)Result<int, string>.NoValue; });
        }

        [Test]
        public void HasValue_ForDefaultValue_ReturnsFalse()
        {
            var val = default(Result<int, string>);
            Assert.IsFalse(val.HasValue);
        }

        [Test]
        public void Equals_WithDefaultValueAndNoValue_ReturnsTrue()
        {
            Result<int, string> val = default(Result<int, string>);
            Assert.IsTrue(val == Result<int, string>.NoValue);
        }

        [Test]
        public void Equals_WithNullValueAndNoValue_ReturnsFalse()
        {
            Assert.IsTrue(new Result<string, int>((string)null) != Result<string, int>.NoValue);
        }

        [Test]
        public void Equals_WithTwoResultsWithSameValue_ReturnsTrue()
        {
            var left = new Result<int, string>(7);
            var right = new Result<int, string>(7);

            Assert.That(left == right);
        }

        [Test]
        public void Equals_UsesNonGenericEqualsOfUnderlyingValue_IfGenericIsNotAvailable()
        {
            var left = new Result<DayOfWeek, string>(DayOfWeek.Friday);
            var right = new Result<DayOfWeek, string>(DayOfWeek.Friday);

            Assert.That(left == right);

            Assert.That(left == DayOfWeek.Friday);
            Assert.That(left != DayOfWeek.Thursday);
            Assert.That(DayOfWeek.Friday == left);
            Assert.That(DayOfWeek.Thursday != left);
        }

        [Test]
        public void Equals_OnBoxedResult_BehavesCorrectly()
        {
            object left = new Result<int, string>(7);
            object right = new Result<int, string>(7);

            Assert.IsTrue(left.Equals(right));
            Assert.IsFalse(left.Equals(null));
            Assert.IsTrue(left.Equals(7));
            Assert.IsFalse(left.Equals(42));
            Assert.IsFalse(left.Equals("Hello World!"));
        }

        [Test]
        public void GetHashCode_OnNoValue_ReturnsNegativeOne()
        {
            Assert.AreEqual(-1, Result<int, string>.NoValue.GetHashCode());
        }

        [Test]
        public void GetHashCode_OnValue_ReturnsUnderlyingHashCode()
        {
            int val = 42;
            Assert.AreEqual(val.GetHashCode(), new Result<int, string>(val).GetHashCode());
        }

        [Test]
        public void GetHashCode_OnNullValue_ReturnsZero()
        {
            Assert.AreEqual(0, new Result<string, int>((string)null).GetHashCode());
        }

        [Test]
        public void EvaluationOfResult_OnlyOccursOnce()
        {
            int count = 0;
            Func<int> funcOfInt = () => ++count;

            var maybe = new Result<int,string>(funcOfInt);

            for (int i = 0; i < 10; i++)
                Assert.AreEqual(1, maybe.Value);

            Func<Result<int, string>> funcOfMaybeOfInt = () => new Result<int, string>(++count);

            count = 0;
            maybe = new Result<int, string>(funcOfMaybeOfInt);

            for (int i = 0; i < 10; i++)
                Assert.AreEqual(1, maybe.Value);
        }
    }
}
