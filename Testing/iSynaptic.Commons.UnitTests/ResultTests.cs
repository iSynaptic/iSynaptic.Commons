// The MIT License
// 
// Copyright (c) 2012-2015 Jordan E. Terrell
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
using System.Linq;
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
            Assert.IsTrue(Result.Return("").HasValue);
        }

        [Test]
        public void AccessingValueProperty_OnValue_ReturnsExpectedValue()
        {
            Assert.AreEqual("Hello, World!", Result.Return("Hello, World!").Value);
        }

        [Test]
        public void Observations_WithNoValue_IsEmpty()
        {
            Assert.IsTrue(Result<int, string>.NoValue.Observations.Count() == 0);
        }

        [Test]
        public void ExplicitCast_OnValue_ReturnsValue()
        {
            int value = (int)Result.Return(42);

            Assert.AreEqual(42, value);
        }

        [Test]
        public void ExplicitCast_OnNoValue_ThrowsException()
        {
            Assert.Throws<InvalidOperationException>(() => { var x = (int)Result<int, string>.NoValue; });
        }

        [Test]
        public void Behavior_ForNonUnitDefaultValue()
        {
            var val = default(Result<int, string>);

            Assert.IsFalse(val.HasValue);
            Assert.IsTrue(val.WasSuccessful);
            Assert.AreEqual(0, val.Observations.Count());
        }

        [Test]
        public void Behavior_ForUnitDefaultValue()
        {
            var val = default(Result<Unit, Unit>);

            Assert.IsTrue(val.HasValue);
            Assert.AreEqual(new Unit(), val.Value);
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
            Assert.IsTrue(Result.Return<string>(null) == Result<string, int>.NoValue);
        }

        [Test]
        public void Equals_WithTwoResultsWithSameValue_ReturnsTrue()
        {
            var left = Result.Return(7);
            var right = Result.Return(7);

            Assert.That(left == right);
        }

        [Test]
        public void Equals_UsesNonGenericEqualsOfUnderlyingValue_IfGenericIsNotAvailable()
        {
            var left = Result.Return(DayOfWeek.Friday);
            var right = Result.Return(DayOfWeek.Friday);

            Assert.That(left == right);

            Assert.That(left == DayOfWeek.Friday);
            Assert.That(left != DayOfWeek.Thursday);
            Assert.That(DayOfWeek.Friday == left);
            Assert.That(DayOfWeek.Thursday != left);
        }

        [Test]
        public void Equals_OnBoxedResult_BehavesCorrectly()
        {
            object left = Result.Return(7);
            object right = Result.Return(7);

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
            Assert.AreEqual(val.GetHashCode() ^ true.GetHashCode(), Result.Return(val).GetHashCode());
        }

        [Test]
        public void GetHashCode_OnNullValue_IncludesValuesHashCode()
        {
            Assert.AreEqual(0 ^ true.GetHashCode(), Result.Return<string>(null).GetHashCode());
        }

        [Test]
        public void EvaluationOfResult_OnlyOccursOnce()
        {
            int count = 0;
            Func<Result<int, string>> funcOfMaybeOfInt = () => Result.Return(++count);

            var maybe = Result.Defer(funcOfMaybeOfInt);

            for (int i = 0; i < 10; i++)
                Assert.AreEqual(1, maybe.Value);
        }

        [Test]
        public void ComputedObservations_AreYielded()
        {
            var result = Result.Defer(() => Result.Success("Hello", "World"));

            Assert.IsTrue(result.Observations.SequenceEqual(new[] {"Hello", "World"}));
        }

        [Test]
        public void StaticObservations_AreYielded()
        {
            var result = Result.Success("Hello", "World");
            Assert.IsTrue(result.Observations.SequenceEqual(new[] { "Hello", "World" }));
        }


        [Test]
        public void OfType_CanConvertObservations()
        {
            IResult<object, object> result = Result.Return("Hello, World!")
                .Observe("Goodbye, World!");

            var converted = result.OfType<string, string>();

            Assert.IsTrue(converted.HasValue);
            Assert.AreEqual(converted.Value, "Hello, World!");

            Assert.IsTrue(converted.WasSuccessful);
            Assert.IsTrue(converted.Observations.SequenceEqual(new[] { "Goodbye, World!" }));
        }

        [Test]
        public void ImplicitConversionOf_NoValue_ToAnyOtherResultType()
        {
            Result<int, string> intStringResult = Result.NoValue;

            Assert.IsFalse(intStringResult.HasValue);
            Assert.IsTrue(intStringResult.WasSuccessful);
            Assert.AreEqual(0, intStringResult.Observations.Count());

            Result<string, int> stringIntResult = Result.NoValue;

            Assert.IsFalse(stringIntResult.HasValue);
            Assert.IsTrue(stringIntResult.WasSuccessful);
            Assert.AreEqual(0, stringIntResult.Observations.Count());
        }

        [Test]
        public void ImplicitConversionOf_ResultTAndUnit_ToAnyOtherResultType()
        {
            Result<int, string> intStringResult = Result.Return(42);

            Assert.IsTrue(intStringResult.HasValue);
            Assert.AreEqual(42, intStringResult.Value);
            Assert.IsTrue(intStringResult.WasSuccessful);
            Assert.AreEqual(0, intStringResult.Observations.Count());
        }

        [Test]
        public void ImplicitConversionOf_ResultUnitAndT_ToAnyOtherResultType()
        {
            Result<int, string> intStringResult = Result.Success("Hello, World!");

            Assert.IsFalse(intStringResult.HasValue);
            Assert.IsTrue(intStringResult.WasSuccessful);
            Assert.IsTrue(intStringResult.Observations.SequenceEqual(new[]{"Hello, World!"}));
        }

        [Test]
        public void ImplicitConversion_WhenObservationTypeIsUnit_DoesNotLooseFailures()
        {
            Result<int, string> intStringResult = Result.Failure();
            Assert.IsFalse(intStringResult.WasSuccessful);

            intStringResult = 42.ToResult().Fail();
            Assert.IsFalse(intStringResult.WasSuccessful);
        }

        [Test]
        public void Success_Default()
        {
            var result = Result.Success();

            Assert.IsTrue(result.HasValue);
            Assert.AreEqual(new Unit(), result.Value);
            Assert.IsTrue(result.WasSuccessful);
            Assert.AreEqual(0, result.Observations.Count());
        }

        [Test]
        public void Success_WithOneObservation()
        {
            var result = Result.Success("Hello");

            Assert.IsTrue(result.HasValue);
            Assert.AreEqual(new Unit(), result.Value);
            Assert.IsTrue(result.WasSuccessful);
            Assert.IsTrue(result.Observations.SequenceEqual(new[]{"Hello"}));
        }

        [Test]
        public void Success_WithMultipleObservation()
        {
            var result = Result.Success("Hello", "World");

            Assert.IsTrue(result.HasValue);
            Assert.AreEqual(new Unit(), result.Value);
            Assert.IsTrue(result.WasSuccessful);
            Assert.IsTrue(result.Observations.SequenceEqual(new[] { "Hello", "World" }));
        }

        [Test]
        public void Failure_Default()
        {
            var result = Result.Failure();

            Assert.IsTrue(result.HasValue);
            Assert.AreEqual(new Unit(), result.Value);
            Assert.IsFalse(result.WasSuccessful);
            Assert.AreEqual(0, result.Observations.Count());
        }

        [Test]
        public void Failure_WithOneObservation()
        {
            var result = Result.Failure("Hello");

            Assert.IsTrue(result.HasValue);
            Assert.AreEqual(new Unit(), result.Value);
            Assert.IsFalse(result.WasSuccessful);
            Assert.IsTrue(result.Observations.SequenceEqual(new[] { "Hello" }));
        }

        [Test]
        public void Failure_WithMultipleObservation()
        {
            var result = Result.Failure("Hello", "World");

            Assert.IsTrue(result.HasValue);
            Assert.AreEqual(new Unit(), result.Value);
            Assert.IsFalse(result.WasSuccessful);
            Assert.IsTrue(result.Observations.SequenceEqual(new[] { "Hello", "World" }));
        }

        [Test]
        public void Observe_AgainstUnitObservation()
        {
            var result = Result.Success()
                .Observe("Hello");

            Assert.IsTrue(result.Observations.SequenceEqual(new[]{"Hello"}));
        }

        [Test]
        public void Observe_AgainstNonUnitObservation()
        {
            var result = new Result<string, string>()
                .Observe("Hello");

            Assert.IsTrue(result.Observations.SequenceEqual(new[] { "Hello" }));
        }

        [Test]
        public void Observe_ViaSelectorAgainstUnitObservation()
        {
            var result = Result.Success()
                .Observe(x => "Hello");

            Assert.IsTrue(result.Observations.SequenceEqual(new[] { "Hello" }));
        }

        [Test]
        public void Observe_ViaSelectorAgainstNonUnitObservation()
        {
            var result = new Result<string, string>()
                .Observe(x => "Hello");

            Assert.IsTrue(result.Observations.SequenceEqual(new[] { "Hello" }));
        }

        [Test]
        public void ObserveMany_AgainstUnitObservation()
        {
            var result = Result.Success()
                .ObserveMany("Hello", "World");

            Assert.IsTrue(result.Observations.SequenceEqual(new[] { "Hello", "World" }));
        }

        [Test]
        public void ObserveMany_AgainstNonUnitObservation()
        {
            var result = new Result<string, string>()
                .ObserveMany("Hello", "World");

            Assert.IsTrue(result.Observations.SequenceEqual(new[] { "Hello", "World" }));
        }

        [Test]
        public void ObserveMany_ViaSelectorAgainstUnitObservation()
        {
            var result = Result.Success()
                .ObserveMany(x => new[]{"Hello", "World"});

            Assert.IsTrue(result.Observations.SequenceEqual(new[] { "Hello", "World" }));
        }

        [Test]
        public void ObserveMany_ViaSelectorAgainstNonUnitObservation()
        {
            var result = new Result<string, string>()
                .ObserveMany(x => new[]{"Hello", "World"});

            Assert.IsTrue(result.Observations.SequenceEqual(new[] { "Hello", "World" }));
        }

        [Test]
        public void Inform()
        {
            var result = Result.Success(1, 2)
                .Inform(x => x.ToString());

            Assert.IsTrue(result.Observations.SequenceEqual(new[]{"1", "2"}));
        }

        [Test]
        public void InformMany()
        {
            var result = Result.Success(1, 2)
                .InformMany(x => Outcome.Success(x.ToString(), (x * 2).ToString()));

            Assert.IsTrue(result.Observations.SequenceEqual(new[] { "1", "2", "2", "4" }));
        }

        [Test]
        public void Ignore()
        {
            var result = Result.Success(1, 2)
                .Ignore(x => x%2 == 0);

            Assert.IsTrue(result.Observations.SequenceEqual(new[]{1}));
        }

        [Test]
        public void Notice()
        {
            var result = Result.Success(1, 2)
                .Notice(x => x % 2 == 0);

            Assert.IsTrue(result.Observations.SequenceEqual(new[] { 2 }));
        }

        [Test]
        public void Combine()
        {
            var result = Result.Success(1, 2)
                .Combine(Outcome.Success(3, 4));

            Assert.IsTrue(result.Observations.SequenceEqual(new[]{1,2,3,4}));
        }

        [Test]
        public void FailIf_AgaistUnitObservationWithSimpleBool()
        {
            var result = Result.Success()
                .FailIf(false);

            Assert.IsTrue(result.WasSuccessful);

            result = result.FailIf(true);
            Assert.IsFalse(result.WasSuccessful);
        }

        [Test]
        public void FailIf_AgaistNonUnitObservationWithSimpleBool()
        {
            var result = Result.Success(1)
                .FailIf(false);

            Assert.IsTrue(result.WasSuccessful);

            result = result.FailIf(true);
            Assert.IsFalse(result.WasSuccessful);
        }

        [Test]
        public void FailIf_AgaistUnitObservationWithBoolPredicate()
        {
            var result = Result.Success()
                .FailIf(() => false);

            Assert.IsTrue(result.WasSuccessful);

            result = result.FailIf(() => true);
            Assert.IsFalse(result.WasSuccessful);
        }

        [Test]
        public void FailIf_AgaistNonUnitObservationWithBoolPredicate()
        {
            var result = Result.Success(1)
                .FailIf(() => false);

            Assert.IsTrue(result.WasSuccessful);

            result = result.FailIf(() => true);
            Assert.IsFalse(result.WasSuccessful);
        }

        [Test]
        public void If_ReturnsThenValue_IfBoolIsTrue()
        {
            var value = Result.If(true, 42.ToResult());
            Assert.That(value == 42);

            //value = Result.If(() => true, 84.ToResult());
            //Assert.That(value == 84);
        }

        [Test]
        public void If_ReturnsElseValue_IfBoolIsFalse()
        {
            var value = Result.If(false, 42.ToResult(), 7.ToResult());
            Assert.That(value == 7);

            //value = Result.If(() => false, 7.ToResult(), 42.ToResult());
            //Assert.That(value == 42);
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
