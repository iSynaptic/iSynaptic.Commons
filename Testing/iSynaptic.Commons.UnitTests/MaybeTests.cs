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
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Rhino.Mocks;

namespace iSynaptic.Commons
{
    [TestFixture]
    public partial class MaybeTests
    {
        #region Maybe<T> Members

        [Test]
        public void AccessingValueProperty_OnNoValue_ThrowsInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() => Console.WriteLine(Maybe<string>.NoValue.Value));
        }

        [Test]
        public void HasValueProperty_OnNoValue_ReturnsFalse()
        {
            Assert.IsFalse(Maybe<string>.NoValue.HasValue);
        }

        [Test]
        public void HasValueProperty_OnValue_ReturnsTrue()
        {
            Assert.IsTrue(Maybe.Return("").HasValue);
        }

        [Test]
        public void AccessingValueProperty_OnValue_ReturnsExpectedValue()
        {
            Assert.AreEqual("Hello, World!", Maybe.Return("Hello, World!").Value);
        }

        [Test]
        public void ExplicitCast_OnValue_ReturnsValue()
        {
            int value = (int) Maybe.Return(42);

            Assert.AreEqual(42, value);
        }

        [Test]
        public void ExplicitCast_OnNoValue_ThrowsException()
        {
            Assert.Throws<InvalidOperationException>(() => { var x = (int) Maybe<int>.NoValue; });
        }

        [Test]
        public void HasValue_ForNonUnitDefaultValue_ReturnsFalse()
        {
            var val = default(Maybe<int>);
            Assert.IsFalse(val.HasValue);
        }

        [Test]
        public void HasValue_ForUnitDefaultValue_ReturnsTrue()
        {
            var val = default(Maybe<Unit>);
            Assert.IsTrue(val.HasValue);
        }


        [Test]
        public void Equals_WithNonUnitDefaultValueAndNoValue_ReturnsTrue()
        {
            Maybe<int> val = default(Maybe<int>);
            Assert.IsTrue(val == Maybe<int>.NoValue);
        }

        [Test]
        public void Equals_WithUnitDefaultValueAndNoValue_ReturnsTrue()
        {
            Maybe<int> val = default(Maybe<Unit>);
            Assert.IsTrue(val == Maybe<int>.NoValue);
        }

        [Test]
        public void Equals_WithNullValueAndNoValue_ReturnsTrue()
        {
            Assert.IsTrue(Maybe.Return<string>(null) == Maybe<string>.NoValue);
        }

        [Test]
        public void Equals_WithTwoMaybesWithSameValue_ReturnsTrue()
        {
            var left = Maybe.Return(7);
            var right = Maybe.Return(7);

            Assert.That(left == right);
        }

        [Test]
        public void Equals_UsesNonGenericEqualsOfUnderlyingValue_IfGenericIsNotAvailable()
        {
            var left = Maybe.Return(DayOfWeek.Friday);
            var right = Maybe.Return(DayOfWeek.Friday);

            Assert.That(left == right);

            Assert.That(left == DayOfWeek.Friday);
            Assert.That(left != DayOfWeek.Thursday);
            Assert.That(DayOfWeek.Friday == left);
            Assert.That(DayOfWeek.Thursday != left);
        }

        [Test]
        public void Equals_OnBoxedMaybe_BehavesCorrectly()
        {
            object left = Maybe.Return(7);
            object right = Maybe.Return(7);

            Assert.IsTrue(left.Equals(right));
            Assert.IsFalse(left.Equals(null));
            Assert.IsTrue(left.Equals(7));
            Assert.IsFalse(left.Equals(42));
            Assert.IsFalse(left.Equals("Hello World!"));
        }

        [Test]
        public void GetHashCode_OnNoValue_ReturnsNegativeOne()
        {
            Assert.AreEqual(0, Maybe<int>.NoValue.GetHashCode());
        }

        [Test]
        public void GetHashCode_OnValue_ReturnsUnderlyingHashCode()
        {
            int val = 42;
            Assert.AreEqual(val.GetHashCode(), Maybe.Return(val).GetHashCode());
        }

        [Test]
        public void GetHashCode_OnNullValue_ReturnsZero()
        {
            Assert.AreEqual(0, Maybe.Return<string>(null).GetHashCode());
        }

        [Test]
        public void EvaluationOfMaybe_OnlyOccursOnce()
        {
            int count = 0;

            Func<Maybe<int>> funcOfMaybeOfInt = () => Maybe.Return(++count);
            var maybe = Maybe.Defer(funcOfMaybeOfInt);

            for (int i = 0; i < 10; i++)
                Assert.AreEqual(1, maybe.Value);
        }

        #endregion

        [Test]
        public void Select_BehavesCorrectly()
        {
            AssertMaybe.Behavior(42, x => x.Select(y => y), AssertMaybeResult.SameValue);
            AssertMaybe.Behavior(42, x => x.Select(y => 5), AssertMaybeResult.Value);
        }

        [Test]
        public void SelectMaybe_BehavesCorrectly()
        {
            AssertMaybe.Behavior(42, x => x.SelectMaybe(Maybe.Return), AssertMaybeResult.SameValue);
            AssertMaybe.Behavior(42, x => x.SelectMaybe(y => Maybe.Return(5)), AssertMaybeResult.Value);
        }

        [Test]
        public void ValueProperty_ThatImplicitlyThrowsException_BubblesUpException()
        {
            var results = 0.ToMaybe()
                .SelectMaybe(x => (7 / x).ToMaybe());

            Assert.Throws<DivideByZeroException>(() => { var x = results.Value; });
        }

        [Test]
        public void HasValueProperty_ThatImplicitlyThrowsException_BubblesUpException()
        {
            var results = 0.ToMaybe()
                .SelectMaybe(x => (7 / x).ToMaybe());

            Assert.Throws<DivideByZeroException>(() => { var x = results.HasValue; });
        }

        [Test]
        public void SelectMaybe_WhenExceptionOccurs_RemainingOperationsDoNotExecute()
        {
            bool executed = false;

            var results = 0.ToMaybe()
                .SelectMaybe(x => (7 / x).ToMaybe())
                .SelectMaybe(x => (executed = true).ToMaybe());

            Assert.Throws<DivideByZeroException>(() => { var x = results.Value; });
            Assert.IsFalse(executed);
        }

        [Test]
        public void SelectMaybe_OnNoValue_RemaningOperationsDoNotExecute()
        {
            bool executed = false;

            var results = 0.ToMaybe()
                 .SelectMaybe(x => (x + 7).ToMaybe())
                 .SelectMaybe(x => Maybe<int>.NoValue)
                 .SelectMaybe(x => (executed = true).ToMaybe());

            Assert.IsFalse(results.HasValue);
            Assert.IsFalse(executed);
        }

        [Test]
        public void Return_ReturnsValueWrapedInMaybe()
        {
            AssertMaybe.Behavior(42, (Expression<Func<int, IMaybe>>)(x => Maybe.Return(x)), AssertMaybeResult.SameValue);
        }

        [Test]
        public void Return_StripsOffNullabilityFromNullableStrucs()
        {
            int? input = null;
            Maybe<int> value = Maybe.Return(input);

            Assert.IsFalse(value.HasValue);
        }

        [Test]
        public void ToMaybe_StripsOffNullabilityFromNullableStrucs()
        {
            int? input = null;
            Maybe<int> value = input.ToMaybe();

            Assert.IsFalse(value.HasValue);
        }

        [Test]
        public void Null_IsTreatedAsNoValue()
        {
            string input = null;
            var value = Maybe.Return(input);

            Assert.IsFalse(value.HasValue);

            int? input2 = null;
            var value2 = Maybe.Return(input2);

            Assert.IsFalse(value2.HasValue);
        }

        [Test]
        public void Select_ReturnsSelectedValueInMaybe()
        {
            AssertMaybe.Behavior("Hello, World!", x => x.Select(y => y), AssertMaybeResult.SameValue);
            AssertMaybe.Behavior(7, x => x.Select(y => y * 6), AssertMaybeResult.Value);
        }

        [Test]
        public void Where_ReturnsValueIfPredicateReturnsTrue()
        {
            string rawValue = "Hello World!";

            var value = Maybe
                .Return(rawValue)
                .Where(x => x.Length == rawValue.Length);

            Assert.IsTrue(value.HasValue);
            Assert.AreEqual(rawValue, value.Value);

            value = Maybe
                .Return(rawValue)
                .Where(x => x.Length == rawValue.Length - 1);

            Assert.IsFalse(value.HasValue);
        }

        [Test]
        public void Unless_ReturnsValueIfPredicateReturnsFalse()
        {
            string rawValue = "Hello World!";

            var value = Maybe
                .Return(rawValue)
                .Unless(x => x.Length == rawValue.Length - 1);

            Assert.IsTrue(value.HasValue);
            Assert.AreEqual(rawValue, value.Value);

            value = Maybe
                .Return(rawValue)
                .Unless(x => x.Length == rawValue.Length);

            Assert.IsFalse(value.HasValue);
        }

        [Test]
        public void ValueOrDefault_UnwrapsValueIfItHasAvalue()
        {
            var rawValue = "Hello World!";
            var value = Maybe.Return(rawValue)
                .ValueOrDefault("{default}");

            Assert.AreEqual(rawValue, value);

            value = Maybe<string>.NoValue
                .ValueOrDefault("{default}");

            Assert.AreEqual("{default}", value);
        }

        [Test]
        public void OnValue_CallsActionIfHasValueIsTrue()
        {
            bool didExecute = false;

            var value = Maybe<string>.NoValue
                .OnValue(x => didExecute = true);

            Assert.IsFalse(value.HasValue);
            Assert.IsFalse(didExecute);

            value = Maybe.Return("Hello World!")
                .OnValue(x => didExecute = true);

            Assert.IsTrue(value.HasValue);
            Assert.AreEqual("Hello World!", value.Value);
            Assert.IsTrue(didExecute);
        }

        [Test]
        public void OnNoValue_CallsActionIfHasValueIsFalse()
        {
            bool didExecute = false;

            var value = "42".ToMaybe()
                .OnNoValue(() => didExecute = true);

            Assert.IsTrue(value.HasValue);
            Assert.AreEqual("42", value.Value);
            Assert.IsFalse(didExecute);

            value = Maybe<string>.NoValue
                .OnNoValue(() => didExecute = true);

            Assert.IsFalse(value.HasValue);
            Assert.IsTrue(didExecute);
        }

        [Test]
        public void OnException_ExecutesActionOnThrownException()
        {
            bool executed = false;

            var value = 0.ToMaybe()
                .Select(x => 7 / x)
                .OnException(x => executed = true);

            Assert.Throws<DivideByZeroException>(() => value.ValueOrDefault());
            Assert.IsTrue(executed);
        }

        [Test]
        public void OnException_OnThrownException_DoesNotExecuteHandlerAgainForExceptionCausedByHandler()
        {
            int count = 0;

            var value = 0.ToMaybe()
                .Select(x => 7 / x)
                .OnException(x => { count++; throw new NullReferenceException(); });

            var exception = Assert.Throws<NullReferenceException>(() => value.ValueOrDefault());
            Assert.AreEqual(1, count);
        }

        [Test]
        public void OnException_DoesNotExecuteHandlerAgainForExceptionCausedByHandler()
        {
            int count = 0;

            var value = Maybe.Throw<string>(new InvalidOperationException())
                .OnException(x => { count++; throw new NullReferenceException(); });

            var exception = Assert.Throws<NullReferenceException>(() => value.ValueOrDefault());
            Assert.AreEqual(1, count);
        }

        [Test]
        public void Assign_AssignsValueToReferenceIfHasValue()
        {
            string rawValue = "Hello World!";

            string refString = null;
            int refInt = 0;

            Maybe<string>.NoValue
                .Assign(ref refString)
                .Select(x => x.Length)
                .Assign(ref refInt);

            Assert.IsNull(refString);
            Assert.AreEqual(0, refInt);

            Maybe.Return(rawValue)
                .Assign(ref refString)
                .Select(x => x.Length)
                .Assign(ref refInt);

            Assert.AreEqual(rawValue, refString);
            Assert.AreEqual(rawValue.Length, refInt);
        }

        [Test]
        public void Or_ContinuesWithNewValue()
        {
            var value = Maybe<int>.NoValue
                .Or(42)
                .Value;

            Assert.AreEqual(42, value);
        }

        [Test]
        public void Or_DoesNotSuppressExceptions()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                var x = Maybe.Throw<int>(new InvalidOperationException())
                    .Or(42)
                    .Value;
            });
        }

        [Test]
        public void Suppress_ContinuesWithNoValue()
        {
            var value = "Hello, World!"
                .ToMaybe()
                .SelectMaybe(x => Maybe.Throw<string>(new InvalidOperationException()))
                .Select(x => x.Length)
                .Where(x => x > 10)
                .Suppress();

            Assert.That(value == Maybe<int>.NoValue);
        }

        [Test]
        public void Suppress_ContinuesWithNewValue()
        {
            var value = "Hello, World!"
                .ToMaybe()
                .SelectMaybe(x => Maybe.Throw<string>(new InvalidOperationException()))
                .Select(x => x.Length)
                .Where(x => x > 10)
                .Suppress(42);

            Assert.That(value == 42);
        }

        [Test]
        public void With_WhenHasValue_Executes()
        {
            int checkValue = 0;

            var value = Maybe.Return("Hello")
                .With(x => x.Length, x => checkValue = x)
                .ValueOrDefault();

            Assert.AreEqual("Hello", value);
            Assert.AreEqual(5, checkValue);
        }

        [Test]
        public void With_WhenNoValue_DoesNotExecutes()
        {
            int checkValue = 0;

            var value = Maybe.Return("Hello")
                .With(x => Maybe<int>.NoValue, x => checkValue = x)
                .ValueOrDefault();

            Assert.AreEqual("Hello", value);
            Assert.AreEqual(0, checkValue);
        }

        [Test]
        public void When_PredicateIsTrue_UsesNewValue()
        {
            var value = Maybe
                .Return("Hello")
                .When("Hello", "World".ToMaybe());

            Assert.IsTrue(value.HasValue);
            Assert.AreEqual("World", value.Value);
        }

        [Test]
        public void When_PredicateIsFalse_UsesOriginalValue()
        {
            var value = Maybe.Return("Hello")
                .When("Goodbye", "World".ToMaybe());

            Assert.IsTrue(value.HasValue);
            Assert.AreEqual("Hello", value.Value);
        }

        [Test]
        public void When_WithAction_ExecutesAction()
        {
            string output = null;

            Maybe.Return("Hello")
                .When("Hello", x => output = x)
                .Run();

            Assert.AreEqual("Hello", output);
        }

        [Test]
        public void Defer_DefersExecutionUntilEvaluated()
        {
            bool executed = false;
            var value = Maybe.Defer(() => { executed = true; return 42; });

            Assert.IsFalse(executed);

            Assert.AreEqual(42, value.Value);
            Assert.IsTrue(executed);
        }

        [Test]
        public void Select_DefersExecutionUntilEvaluated()
        {
            bool executed = false;
            var value = Maybe.Defer(() => { executed = true; return "42"; })
                .Select(x => x.Length);

            Assert.IsFalse(executed);

            Assert.AreEqual(2, value.Value);
            Assert.IsTrue(executed);
        }

        [Test]
        public void SelectMaybe_DefersExecutionUntilEvaluated()
        {
            bool executed = false;
            Maybe<int> value = Maybe.Defer(() => { executed = true; return "42"; })
                .SelectMaybe(x => Maybe<int>.NoValue);

            Assert.IsFalse(executed);

            Assert.IsFalse(value.HasValue);
            Assert.IsTrue(executed);
        }

        [Test]
        public void Synchronize_PreventsMultipleEvaluation()
        {
            int count = 0;
            var value = Maybe.Defer(() => { count++; Thread.Sleep(250); return "42"; })
                .Synchronize();

            string results = null;

            var t1 = Task.Factory.StartNew(() => { results = value.Value; });
            var t2 = Task.Factory.StartNew(() => { results = value.Value; });

            Task.WaitAll(t1, t2);

            Assert.AreEqual("42", results);
            Assert.AreEqual(1, count);
        }

        [Test]
        public void Where_DefersExecutionUntilEvaluated()
        {
            bool executed = false;
            var value = Maybe.Defer(() => { executed = true; return "42"; })
                .Where(x => x.Length == 2);

            Assert.IsFalse(executed);

            Assert.AreEqual("42", value.Value);
            Assert.IsTrue(executed);
        }

        [Test]
        public void Unless_DefersExecutionUntilEvaluated()
        {
            bool executed = false;
            var value = Maybe.Defer(() => { executed = true; return "42"; })
                .Unless(x => x.Length == 2);

            Assert.IsFalse(executed);

            Assert.IsFalse(value.HasValue);
            Assert.IsTrue(executed);
        }

        [Test]
        public void OnValue_DefersExecutionUntilEvaluated()
        {
            bool executed = false;
            var value = Maybe.Defer(() => { executed = true; return "42"; })
                .OnValue(x => executed = true);

            Assert.IsFalse(executed);

            Assert.AreEqual("42", value.Value);
            Assert.IsTrue(executed);
        }

        [Test]
        public void Assign_ExecutesImmediately()
        {
            bool executed = false;
            Maybe.Return(true)
                .Assign(ref executed);

            Assert.IsTrue(executed);
        }

        [Test]
        public void Suppress_DefersExecutionUntilEvaluated()
        {
            bool executed = false;
            var value = Maybe.Defer(() => { executed = true; return "42"; })
                .OnValue(x => executed = true)
                .Suppress("Hello");

            Assert.IsFalse(executed);

            Assert.AreEqual("42", value.Value);
            Assert.IsTrue(executed);
        }

        [Test]
        public void ThrowOnNoValue_DefersExecutionUntilEvaluated()
        {
            bool executed = false;
            Maybe<int> value = Maybe.Defer(() => { executed = true; return Maybe<int>.NoValue; })
                .ThrowOnNoValue(new InvalidOperationException());

            Assert.IsFalse(executed);

            Assert.Throws<InvalidOperationException>(() => { var notAssigned = value.HasValue; });
            Assert.IsTrue(executed);
        }

        [Test]
        public void ThrowOn_DefersExecutionUntilEvaluated()
        {
            bool executed = false;
            Maybe<int> value = Maybe.Defer(() => { executed = true; return 42.ToMaybe(); })
                .ThrowOn(42, new InvalidOperationException());

            Assert.IsFalse(executed);

            Assert.Throws<InvalidOperationException>(() => { var notAssigned = value.HasValue; });
            Assert.IsTrue(executed);
        }

        [Test]
        public void Synchronize_DeferesExecutionUntilEvaluated()
        {
            bool executed = false;
            var value = Maybe.Defer(() => { executed = true; return 42; })
                .Synchronize();

            Assert.IsFalse(executed);

            Assert.AreEqual(42, value.Value);
            Assert.IsTrue(executed);
        }

        [Test]
        public void Suppress_OnException_ReturnsNoValue()
        {
            var value = 0
                .ToMaybe()
                .Select(x => 7 / x)
                .Suppress()
                .Run();

            Assert.IsFalse(value.HasValue);
        }

        [Test]
        public void Suppress_DoesntSuppressIfPredicateReturnsFalse()
        {
            var value = 0
                .ToMaybe()
                .Select(x => 7 / x)
                .Suppress(x => false);

            Assert.Throws<DivideByZeroException>(() => value.Run());
        }

        [Test]
        public void Cast_WithNullIMaybe_ReturnsNoValue()
        {
            IMaybe value = null;

            Assert.IsTrue(Maybe<object>.NoValue == value.Cast<object>());
        }

        [Test]
        public void Cast_ReturnsCastedType()
        {
            object foo = new List<string>();

            ICollection<string> value = Maybe.Return(foo)
                .Cast<ICollection<string>>()
                .Value;

            Assert.IsTrue(ReferenceEquals(foo, value));
        }

        [Test]
        public void Cast_ToTheSameTypeIsPassThru()
        {
            List<string> foo = new List<string>();

            List<string> value = ((IMaybe)Maybe.Return(foo))
                .Cast<List<string>>()
                .Value;

            Assert.IsTrue(ReferenceEquals(foo, value));
        }

        [Test]
        public void Cast_ThrowsException_WhenCastIsNotPossible()
        {
            object foo = new List<string>();

            var value = Maybe.Return(foo)
                .Cast<DateTime>();

            Assert.Throws<InvalidCastException>(() => { var x = value.HasValue; });
        }

        [Test]
        public void Cast_PropigatesNoValue()
        {
            var value = Maybe<ICollection<string>>.NoValue
                .Cast<DateTime>();

            Assert.IsFalse(value.HasValue);
        }

        [Test]
        public void Cast_DeferesExecutionUntilEvaluated()
        {
            object foo = new List<string>();

            bool executed = false;
            var value = Maybe.Defer(() => { executed = true; return foo; })
                .Cast<ICollection<string>>();

            Assert.IsFalse(executed);

            Assert.IsTrue(ReferenceEquals(foo, value.Value));
            Assert.IsTrue(executed);
        }

        [Test]
        public void OfType_WithNullIMaybe_ReturnsNoValue()
        {
            IMaybe value = null;

            Assert.IsTrue(Maybe<object>.NoValue == value.OfType<object>());
        }

        [Test]
        public void OfType_ToTheSameTypeIsPassThru()
        {
            List<string> foo = new List<string>();

            List<string> value = ((IMaybe)Maybe.Return(foo))
                .OfType<List<string>>()
                .Value;

            Assert.IsTrue(ReferenceEquals(foo, value));
        }

        [Test]
        public void OfType_ReturnsValueAsType()
        {
            object foo = new List<string>();

            ICollection<string> value = Maybe.Return(foo)
                .OfType<ICollection<string>>()
                .Value;

            Assert.IsTrue(ReferenceEquals(foo, value));
        }

        [Test]
        public void OfType_PropigatesNoValue()
        {
            var value = Maybe<ICollection<string>>.NoValue
                .OfType<DateTime>();

            Assert.IsFalse(value.HasValue);
        }

        [Test]
        public void OfType_ReturnsNoValue_WhenCastIsNotPossible()
        {
            object foo = new List<string>();

            var value = Maybe.Return(foo)
                .OfType<DateTime>();

            Assert.IsTrue(value == Maybe<DateTime>.NoValue);
        }

        [Test]
        public void OfType_DeferesExecutionUntilEvaluated()
        {
            object foo = new List<string>();

            bool executed = false;
            var value = Maybe.Defer(() => { executed = true; return foo; })
                .OfType<ICollection<string>>();

            Assert.IsFalse(executed);

            Assert.IsTrue(ReferenceEquals(foo, value.Value));
            Assert.IsTrue(executed);
        }

        [Test]
        public void Or_ReturnsFirstValueIfHasValue()
        {
            var value = Maybe.Return(1)
                .Or(Maybe.Return(42))
                .ValueOrDefault();

            Assert.AreEqual(1, value);
        }

        [Test]
        public void Or_ReturnsSecondValueIfFirstDoesNotHaveValue()
        {
            var value = Maybe<int>.NoValue
                .Or(Maybe.Return(42))
                .ValueOrDefault();

            Assert.AreEqual(42, value);
        }

        [Test]
        public void Or_NoValue_IfBothAreNoValue()
        {
            var value = Maybe<int>.NoValue
                .Or(Maybe<int>.NoValue);

            Assert.IsFalse(value.HasValue);
        }

        [Test]
        public void Join_ReturnsBothValueIfHasValue()
        {
            var value = Maybe.Return(1)
                .Join(Maybe.Return(42))
                .ValueOrDefault();

            Assert.AreEqual(Tuple.Create(1, 42), value);
        }

        [Test]
        public void Join_ReturnsNoValueIfFirstDoesNotHaveValue()
        {
            var value = Maybe<int>.NoValue
                .Join(Maybe.Return(42));

            Assert.IsFalse(value.HasValue);
        }

        [Test]
        public void Join_DoesEvaluateSecondMaybeIfFirstDoesNotHaveValue()
        {
            bool executed = false;
            var value = Maybe<int>.NoValue
                .Join(Maybe.Defer(() => { executed = true; return 42; }));
                
            Assert.IsFalse(value.HasValue);
            Assert.IsFalse(executed);
        }

        [Test]
        public void TrySelect_ReturnsValueIfSelectorReturnsTrue()
        {
            var value = Maybe.Return("42")
                .TrySelect<string, int>(int.TryParse);

            Assert.IsTrue(value.HasValue);
            Assert.AreEqual(42, value.Value);
        }

        [Test]
        public void TrySelect_ReturnsNoValueIfSelectorReturnsFalse()
        {
            var value = Maybe.Return("Hello, World!")
                .TrySelect<string, int>(int.TryParse);

            Assert.IsFalse(value.HasValue);
        }

        [Test]
        public void TrySelect_NoValueThru()
        {
            var value = Maybe<string>.NoValue
                .TrySelect<string, int>(int.TryParse);

            Assert.IsFalse(value.HasValue);
        }

        [Test]
        public void ToNullable_WithNoValue_ReturnsNull()
        {
            var value = Maybe<int>.NoValue
                .ToNullable();

            Assert.IsFalse(value.HasValue);
        }

        [Test]
        public void ToNullable_WithValue_ReturnsValue()
        {
            var value = Maybe.Return(42)
                .ToNullable();

            Assert.AreEqual(42, value.Value);
        }

        [Test]
        public void ToMaybe_WhenValueIsOfTypeT_CastsCorrectly()
        {
            Base value = new Derived();

            var result = value.ToMaybe<Derived>();
            Assert.IsTrue(result.HasValue);
            Assert.AreEqual(value, result.Value);
        }

        [Test]
        public void ToMaybe_WhenValueIsNotOfTypeT_ReturnsNoValue()
        {
            var result = 42.ToMaybe<string>();
            Assert.IsFalse(result.HasValue);
        }

        [Test]
        public void ToMaybe_WhenValueIsNullAndTIsValueType_ReturnsNoValue()
        {
            object value = null;
            var result = value.ToMaybe<int>();

            Assert.IsFalse(result.HasValue);
        }

        [Test]
        public void ToMaybe_WhenValueIsNullAndTIsReferenceType_ReturnsNoValue()
        {
            object value = null;
            var result = value.ToMaybe<string>();

            Assert.IsFalse(result.HasValue);
        }

        [Test]
        public void Using_DisposesResource()
        {
            bool disposed = false;
            Action disposer = () => disposed = true;

            Maybe
                .Using(() => disposer.ToDisposable(), x => Maybe<int>.NoValue)
                .Run();

            Assert.IsTrue(disposed);
        }

        [Test]
        public void Using_DefersExecution()
        {
            bool disposed = false;
            Action disposer = () => disposed = true;

            var value = Maybe
                .Using(() => disposer.ToDisposable(), x => Maybe<int>.NoValue);

            Assert.IsFalse(disposed);

            value.Run();
            Assert.IsTrue(disposed);
        }

        [Test]
        public void RunAsync_ReturnsImmediately()
        {
            int started = 0;
            int ended = 0;
            bool actionExecuted = false;

            var waitEvent = new ManualResetEventSlim();
            var value = Maybe.Return(42)
                .OnValue(x => started = 1)
                .Select(x =>
                        {
                            waitEvent.Wait();
                            return x;
                        })
                .OnValue(x => ended = 1);

            Assert.AreEqual(0, started);
            Assert.AreEqual(0, ended);

            value = value.RunAsync(x => actionExecuted = true);

            var waitForStarted = Task.Factory.StartNew(() => { while (Thread.VolatileRead(ref started) != 1) continue; });
            Assert.IsTrue(waitForStarted.Wait(TimeSpan.FromSeconds(0.5)), "Wait for started failed.");

            Assert.AreEqual(1, started);
            Assert.AreEqual(0, ended);
            waitEvent.Set();

            var waitForEnded = Task.Factory.StartNew(() => { while (Thread.VolatileRead(ref ended) != 1) continue; });
            Assert.IsTrue(waitForEnded.Wait(TimeSpan.FromSeconds(0.5)), "Wait for ended failed.");

            Assert.AreEqual(1, started);
            Assert.AreEqual(1, ended);

            Assert.AreEqual(42, value.Value);
            Assert.IsTrue(actionExecuted);
        }

        [Test]
        public void If_ReturnsNoValue_IfBoolIsFalseAndNoElseValueProvided()
        {
            var value = Maybe.If(false, 42.ToMaybe());
            Assert.IsFalse(value.HasValue);

            value = Maybe.If(() => false, 42.ToMaybe());
            Assert.IsFalse(value.HasValue);
        }

        [Test]
        public void If_ReturnsThenValue_IfBoolIsTrue()
        {
            var value = Maybe.If(true, 42.ToMaybe());
            Assert.That(value == 42);

            value = Maybe.If(() => true, 84.ToMaybe());
            Assert.That(value == 84);
        }

        [Test]
        public void If_ReturnsElseValue_IfBoolIsFalse()
        {
            var value = Maybe.If(false, 42.ToMaybe(), 7.ToMaybe());
            Assert.That(value == 7);

            value = Maybe.If(() => false, 7.ToMaybe(), 42.ToMaybe());
            Assert.That(value == 42);
        }

        [Test]
        public void If_DefersExecutionUntilEvaluated_IfBoolIsTrue()
        {
            bool executed = false;
            var defered = Maybe.Defer(() => { executed = true; return "42"; });

            var value = Maybe.If(true, defered, defered);
            value = Maybe.If(() => true, defered, defered);

            Assert.IsFalse(executed);

            Assert.AreEqual("42", value.Value);
            Assert.IsTrue(executed);
        }

        [Test]
        public void If_DefersExecutionUntilEvaluated_IfBoolIsFalse()
        {
            bool executed = false;
            var defered = Maybe.Defer(() => { executed = true; return "42"; });

            var value = Maybe.If(false, defered, defered);
            value = Maybe.If(() => false, defered, defered);

            Assert.IsFalse(executed);

            Assert.AreEqual("42", value.Value);
            Assert.IsTrue(executed);
        }

        [Test]
        public void Finally_CallsActionUponSuccessfulEvaluation()
        {
            bool disposed = false;
            Action disposer = () => disposed = true;
            var disposable = disposer.ToDisposable();

            var value = disposable.ToMaybe()
                .Select(x => 42)
                .Finally(disposable.Dispose);

            Assert.IsFalse(disposed);

            var result = value.Value;
            Assert.IsTrue(disposed);
            Assert.AreEqual(42, result);
        }

        [Test]
        public void Finally_CallsActionUponFailedEvaluation()
        {
            bool disposed = false;
            Action disposer = () => disposed = true;
            var disposable = disposer.ToDisposable();

            var value = disposable.ToMaybe()
                .ThrowOn(x => new InvalidOperationException())
                .Finally(disposable.Dispose);

            Assert.IsFalse(disposed);

            Assert.Throws<InvalidOperationException>(() => { var result = value.Value; });
            Assert.IsTrue(disposed);
        }

        [Test]
        public void ToEnumerable_OnValue_YieldsSingleValueStream()
        {
            var enumerable = 42.ToMaybe()
                .ToEnumerable();

            Assert.IsTrue(enumerable.SequenceEqual(new[] { 42 }));
        }

        [Test]
        public void ToEnumerable_NoValue_YieldsEmptyStream()
        {
            var enumerable = Maybe<int>.NoValue
                .ToEnumerable();

            Assert.AreEqual(0, enumerable.Count());
        }

        [Test]
        public void ToEnumerable_WithMultipleValues_YieldsValueStream()
        {
            var enumerable = Maybe.ToEnumerable(1.ToMaybe(), 7.ToMaybe(), 42.ToMaybe());

            Assert.IsTrue(enumerable.SequenceEqual(new[] { 1, 7, 42 }));
        }

        [Test]
        public void ToEnumerable_WithSomeNoValues_YieldsAvailableValueStream()
        {
            var enumerable = Maybe.ToEnumerable(1.ToMaybe(), Maybe<int>.NoValue, 42.ToMaybe());

            Assert.IsTrue(enumerable.SequenceEqual(new[] { 1, 42 }));
        }

        [Test]
        public void ToEnumerable_WithSomeExceptionValues_ThrowsException()
        {
            var enumerable = Maybe.ToEnumerable(1.ToMaybe(), Maybe.Throw<int>(new InvalidOperationException()), 42.ToMaybe());

            Assert.Throws<InvalidOperationException>(() => enumerable.ToArray());
        }

        [Test]
        public void AsMaybeOfT_ConvertsIMaybeOfTToStruct()
        {
            var iMaybe = MockRepository.GenerateStub<IMaybe<string>>();
            iMaybe.Stub(x => x.HasValue).Return(true).Repeat.Any();
            ((IMaybe)iMaybe).Stub(x => x.Value).Return("42").Repeat.Any();

            var value = iMaybe.AsMaybe();

            Assert.IsTrue(value.HasValue);
            Assert.AreEqual("42", value.Value);
        }

        [Test]
        public void AsMaybe_ConvertsIMaybeToStruct()
        {
            var iMaybe = MockRepository.GenerateStub<IMaybe>();
            iMaybe.Stub(x => x.HasValue).Return(true).Repeat.Any();
            iMaybe.Stub(x => x.Value).Return("42").Repeat.Any();

            var value = iMaybe.AsMaybe();

            Assert.IsTrue(value.HasValue);
            Assert.AreEqual("42", value.Value);
        }

        [Test]
        public void Squash_WithNullOuterIMaybe_ReturnsNoValue()
        {
            IMaybe<IMaybe<int>> value = null;

            Assert.IsTrue(Maybe<int>.NoValue == value.Squash());
        }

        [Test]
        public void Squash_WithNullInnerIMaybe_ReturnsNoValue()
        {
            IMaybe<IMaybe<int>> value = Maybe.Return<IMaybe<int>>(null);

            Assert.IsTrue(Maybe<int>.NoValue == value.Squash());
        }

        [Test]
        public void Squash_WithNoValueOuterIMaybe_ReturnsNoValue()
        {
            IMaybe<IMaybe<int>> value = Maybe<IMaybe<int>>.NoValue;

            Assert.IsTrue(Maybe<int>.NoValue == value.Squash());
        }

        [Test]
        public void Squash_WithNoValueInnerIMaybe_ReturnsNoValue()
        {
            IMaybe<IMaybe<int>> value = Maybe.Return<IMaybe<int>>(Maybe<int>.NoValue);

            Assert.IsTrue(Maybe<int>.NoValue == value.Squash());
        }

        [Test]
        public void Squash_WithValue_ReturnsValue()
        {
            IMaybe<IMaybe<int>> value = Maybe.Return<IMaybe<int>>(42.ToMaybe());

            var result = value.Squash();

            Assert.IsTrue(result.HasValue);
            Assert.AreEqual(42, result.Value);
        }

        public class Base{}
        public class Derived : Base{}
    }

    internal static class AssertMaybe
    {
        public static void Behavior<T>(T value, Expression<Func<Maybe<T>, IMaybe>> opExpression, AssertMaybeResult withValue, AssertMaybeResult withNoValue = AssertMaybeResult.NoValue)
        {
            Guard.NotNull(opExpression, "opExpression");
            var op = opExpression.Compile();

            var body = opExpression.Body;

            if (body.NodeType == ExpressionType.Convert)
                body = ((UnaryExpression)body).Operand;

            string expression = body.ToString();

            var maybeWithValue = Maybe.Return(value);

            EnsureBehavior(string.Format("{0} with a value", expression), maybeWithValue, () => op(maybeWithValue), withValue);
            EnsureBehavior(string.Format("{0} with no value", expression), Maybe<T>.NoValue, () => op(Maybe<T>.NoValue), withNoValue);
        }

        public static void Behavior<T>(T value, Expression<Func<T, IMaybe>> opExpression, AssertMaybeResult result)
        {
            Guard.NotNull(opExpression, "opExpression");
            var op = opExpression.Compile();

            var body = opExpression.Body;

            if (body.NodeType == ExpressionType.Convert)
                body = ((UnaryExpression)body).Operand;

            string expression = body.ToString();

            EnsureBehavior(string.Format("{0} with {1}", expression, value != null ? value.ToString() : "{null}"), Maybe.Return(value), () => op(value), result);
        }

        private static void EnsureBehavior<T>(string name, Maybe<T> value, Func<IMaybe> op, AssertMaybeResult expectedResult)
        {
            IMaybe result = null;
            try
            {
                result = op();
            }
            catch (Exception)
            {
                if (expectedResult != AssertMaybeResult.ThrowExceptionImmediately)
                    Assert.Fail(string.Format("{0} threw an unexpected immediate exception.", name));

                return;
            }

            if(expectedResult == AssertMaybeResult.ThrowExceptionImmediately)
                Assert.Fail(string.Format("{0} did not throw an exception immediately.", name));

            if(expectedResult == AssertMaybeResult.ThrowException)
            {
                try
                {
                    var discard = result.HasValue;
                    Assert.Fail(string.Format("{0} did not throw an exception upon evaluation.", name));
                }
                catch (Exception)
                {
                    return;
                }
            }


            if (expectedResult == AssertMaybeResult.NoValue && result.HasValue)
                Assert.Fail(string.Format("{0} did not result in the absence of a value.", name));

            if (expectedResult == AssertMaybeResult.Value && !result.HasValue)
                Assert.Fail(string.Format("{0} did not result in a value.", name));

            if (expectedResult == AssertMaybeResult.SameValue && (!result.HasValue || !EqualityComparer<T>.Default.Equals((T)result.Value, value.Value)))
                Assert.Fail(string.Format("{0} did not result in the same value.", name));
        }
    }

    public enum AssertMaybeResult
    {
        SameValue,
        Value,
        NoValue,
        ThrowException,
        ThrowExceptionImmediately
    }
}

namespace iSynaptic.Commons
{
    using Syntax;

    public partial class MaybeTests
    {
        [Test]
        public void ComprehensionSyntaxIsWorking()
        {
            var value = from x in 6.ToMaybe()
                        from y in 7.ToMaybe()
                        let ultimateAnswer = x * y
                        where ultimateAnswer == 42
                        select ultimateAnswer;

            Assert.IsTrue(value.HasValue);
            Assert.AreEqual(42, value.Value);
        }
    }
}