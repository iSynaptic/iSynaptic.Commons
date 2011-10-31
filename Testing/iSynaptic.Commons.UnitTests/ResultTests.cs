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
    public partial class ResultTests
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
            Assert.AreEqual("Hello, World!", new Result<string, int>("Hello, World!").Value);
        }

        [Test]
        public void Observations_WithNoValue_IsEmpty()
        {
            Assert.IsTrue(Result<int, string>.NoValue.Observations.Count() == 0);
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
        public void Behavior_ForDefaultValue()
        {
            var val = default(Result<int, string>);

            Assert.IsFalse(val.HasValue);
            Assert.IsTrue(val.WasSuccessful);
            Assert.AreEqual(0, val.Observations.Count());
        }

        [Test]
        public void Equals_WithDefaultValueAndNoValue_ReturnsTrue()
        {
            Result<int, string> val = default(Result<int, string>);
            Assert.IsTrue(val == Result<int, string>.NoValue);
        }

        [Test]
        public void Equals_WithNullValueAndNoValue_ReturnsTrue()
        {
            Assert.IsTrue(new Result<string, int>((string)null) == Result<string, int>.NoValue);
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
        public void GetHashCode_OnNoValue_IncludesValuesHashCode()
        {
            Assert.AreEqual(0 ^ true.GetHashCode(), Result<int, string>.NoValue.GetHashCode());
        }

        [Test]
        public void GetHashCode_OnValue_IncludesValuesHashCode()
        {
            int val = 42;
            Assert.AreEqual(val.GetHashCode() ^ true.GetHashCode(), new Result<int, string>(val).GetHashCode());
        }

        [Test]
        public void GetHashCode_OnNullValue_IncludesValuesHashCode()
        {
            Assert.AreEqual(0 ^ true.GetHashCode(), new Result<string, int>((string)null).GetHashCode());
        }

        [Test]
        public void EvaluationOfResult_OnlyOccursOnce()
        {
            int count = 0;
            Func<Result<int, string>> funcOfMaybeOfInt = () => new Result<int, string>(++count);

            var maybe = new Result<int, string>(funcOfMaybeOfInt);

            for (int i = 0; i < 10; i++)
                Assert.AreEqual(1, maybe.Value);
        }

        [Test]
        public void ComputedObservations_AreYielded()
        {
            var result = new Result<int, string>(() => new Result<int, string>(new []{"Hello", "World"}));

            Assert.IsTrue(result.Observations.SequenceEqual(new[] {"Hello", "World"}));
        }

        [Test]
        public void StaticObservations_AreYielded()
        {
            var result = new Result<int, string>(new[] { "Hello", "World" });
            Assert.IsTrue(result.Observations.SequenceEqual(new[] { "Hello", "World" }));
        }


        [Test]
        public void OfType_CanConvertObservations()
        {
            IResult<object, object> result = Result.Return<string, string>("Hello, World!")
                .Observe("Goodbye, World!");

            var converted = result.OfType<string, string>();

            Assert.IsTrue(converted.HasValue);
            Assert.AreEqual(converted.Value, "Hello, World!");

            Assert.IsTrue(converted.WasSuccessful);
            Assert.IsTrue(converted.Observations.SequenceEqual(new[] { "Goodbye, World!" }));
        }
    }
}

namespace iSynaptic.Commons
{
    using Syntax;

    public partial class ResultTests
    {
        [Test]
        public void ComprehensionSyntaxIsWorking()
        {
            var value = from x in 6.ToResult<int, Unit>()
                        from y in 7.ToResult<int, Unit>()
                        let ultimateAnswer = x * y
                        where ultimateAnswer == 42
                        select ultimateAnswer;

            Assert.IsTrue(value.HasValue);
            Assert.AreEqual(42, value.Value);
        }
    }
}
