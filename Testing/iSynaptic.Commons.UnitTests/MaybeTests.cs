using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Rhino.Mocks;

namespace iSynaptic.Commons
{
    [TestFixture]
    public class MaybeTests
    {
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
            Assert.IsNull(Maybe.Return<string>(null).Value);
            Assert.AreEqual("Hello, World!", Maybe.Return("Hello, World!").Value);
        }

        [Test]
        public void AccessingValueProperty_OnException_ThrowsException()
        {
            var value = Maybe.Throw<string>(new InvalidOperationException("42"));

            var exception = Assert.Throws<InvalidOperationException>(() => { var x = value.Value; });
            Assert.AreEqual("42", exception.Message);
        }

        [Test]
        public void ExplicitCast_OnValue_ReturnsValue()
        {
            int value = (int) 42.ToMaybe();

            Assert.AreEqual(42, value);
        }

        [Test]
        public void ExplicitCast_OnNoValue_ThrowsException()
        {
            Assert.Throws<InvalidOperationException>(() => { var x = (int) Maybe<int>.NoValue; });
        }

        [Test]
        public void ExplicitCast_OnException_ThrowsWrappedException()
        {
            Assert.Throws<NullReferenceException>(() => { var x = (int) new Maybe<int>(new NullReferenceException()); });
        }

        [Test]
        public void HasValue_ForDefaultValue_ReturnsFalse()
        {
            var val = default(Maybe<int>);
            Assert.IsFalse(val.HasValue);
        }

        [Test]
        public void Equals_WithDefaultValueAndNoValue_ReturnsTrue()
        {
            Maybe<int> val = default(Maybe<int>);
            Assert.IsTrue(val == Maybe<int>.NoValue);
        }

        [Test]
        public void Equals_WithNullValueAndNoValue_ReturnsFalse()
        {
            Assert.IsTrue(Maybe.Return<string>(null) != Maybe<string>.NoValue);
        }

        [Test]
        public void Equals_WithTwoMaybesWithSameValue_ReturnsTrue()
        {
            var left = 7.ToMaybe();
            var right = 7.ToMaybe();

            Assert.That(left == right);
        }

        [Test]
        public void Equals_UsesNonGenericEqualsOfUnderlyingValue_IfGenericIsNotAvailable()
        {
            var left = DayOfWeek.Friday.ToMaybe();
            var right = DayOfWeek.Friday.ToMaybe();

            Assert.That(left == right);

            Assert.That(left == DayOfWeek.Friday);
            Assert.That(left != DayOfWeek.Thursday);
            Assert.That(DayOfWeek.Friday == left);
            Assert.That(DayOfWeek.Thursday != left);
        }

        [Test]
        public void Equals_OnBoxedMaybe_BehavesCorrectly()
        {
            object left = 7.ToMaybe();
            object right = 7.ToMaybe();

            Assert.IsTrue(left.Equals(right));
            Assert.IsFalse(left.Equals(null));
            Assert.IsTrue(left.Equals(7));
            Assert.IsFalse(left.Equals(42));
            Assert.IsFalse(left.Equals("Hello World!"));
        }

        [Test]
        public void GetHashCode_OnNoValue_ReturnsNegativeOne()
        {
            Assert.AreEqual(-1, Maybe<int>.NoValue.GetHashCode());
        }

        [Test]
        public void GetHashCode_OnValue_ReturnsUnderlyingHashCode()
        {
            int val = 42;
            Assert.AreEqual(val.GetHashCode(), new Maybe<int>(val).GetHashCode());
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
            Func<int> funcOfInt = () =>  ++count;

            var maybe = new Maybe<int>(funcOfInt);

            for(int i = 0; i < 10; i++)
                Assert.AreEqual(1, maybe.Value);

            Func<Maybe<int>> funcOfMaybeOfInt = () => (++count).ToMaybe();

            count = 0;
            maybe = new Maybe<int>(funcOfMaybeOfInt);

            for (int i = 0; i < 10; i++)
                Assert.AreEqual(1, maybe.Value);

            count = 0;
            maybe = new Maybe<int>(funcOfInt)
                .Select(x => ++count);

            for (int i = 0; i < 10; i++)
                Assert.AreEqual(2, maybe.Value);
        }

        [Test]
        public void Select_ValueReturnsCorrectly()
        {
            var results = 0.ToMaybe()
                .Select(x => x + 7)
                .Select(x => x * 6);

            Assert.AreEqual(42, results.Value);
        }

        [Test]
        public void SelectMaybe_ValueReturnsCorrectly()
        {
            var results = 0.ToMaybe()
                .SelectMaybe(x => (x + 7).ToMaybe())
                .SelectMaybe(x => (x * 6).ToMaybe());

            Assert.AreEqual(42, results.Value);
        }

        [Test]
        public void ValueProperty_ThatImplicitlyThrowsException_BubblesUpException()
        {
            var results = 0.ToMaybe()
                .Bind(x => (7 / x).ToMaybe());

            Assert.Throws<DivideByZeroException>(() => { var x = results.Value; });
        }

        [Test]
        public void HasValueProperty_ThatImplicitlyThrowsException_BubblesUpException()
        {
            var results = 0.ToMaybe()
                .Bind(x => (7 / x).ToMaybe());

            Assert.Throws<DivideByZeroException>(() => { var x = results.HasValue; });
        }

        [Test]
        public void ExceptionProperty_ThatImplicitlyThrowsException_BubblesUpException()
        {
            var results = 0.ToMaybe()
                .Bind(x => (7 / x).ToMaybe());

            Assert.Throws<DivideByZeroException>(() => { var x = results.Exception; });
        }

        [Test]
        public void Bind_WhenExceptionOccurs_RemainingOperationsDoNotExecute()
        {
            bool executed = false;

            var results = 0.ToMaybe()
                .Bind(x => (7 / x).ToMaybe())
                .Bind(x => (executed = true).ToMaybe());

            Assert.Throws<DivideByZeroException>(() => { var x = results.Value; });
            Assert.IsFalse(executed);
        }

        [Test]
        public void Bind_OnNoValue_RemaningOperationsDoNotExecute()
        {
            bool executed = false;

           var results = 0.ToMaybe()
                .Bind(x => (x + 7).ToMaybe())
                .Bind(x => Maybe<int>.NoValue)
                .Bind(x => (executed = true).ToMaybe());

            Assert.IsFalse(results.HasValue);
            Assert.IsFalse(executed);
        }

        [Test]
        public void ComprehensionSyntaxIsWorking()
        {
            var value = from x in 6.ToMaybe()
                        from y in 7.ToMaybe()
                        let ultimateAnswer = x*y
                        where ultimateAnswer == 42
                        select ultimateAnswer;

            Assert.IsTrue(value.HasValue);
            Assert.IsNull(value.Exception);
            Assert.AreEqual(42, value.Value);
        }

        [Test]
        public void Equals_WithSameExceptionReference_ReturnsTrue()
        {
            var exception = new DivideByZeroException();

            var first = new Maybe<int>(exception);
            var second = new Maybe<int>(exception);

            Assert.IsTrue(first.Equals(second));
        }

        [Test]
        public void Equals_WithDifferentExceptionReference_ReturnsFalse()
        {
            var first = new Maybe<int>(new DivideByZeroException());
            var second = new Maybe<int>(new DivideByZeroException());

            Assert.IsFalse(first.Equals(second));
        }

        [Test]
        public void Equals_WithOneValueAndOneException_ReturnsFalse()
        {
            var result = new Maybe<int>(new DivideByZeroException());
            Assert.IsFalse(0.ToMaybe().Equals(result));
        }

        [Test]
        public void GetHashCode_WithException_ReturnsExceptionsHashCode()
        {
            var exception = new DivideByZeroException();
            var result = new Maybe<int>(exception);

            Assert.AreEqual(exception.GetHashCode(), result.GetHashCode());
        }

        [Test]
        public void NotNull_WithNullReferenceType_ReturnsNoValue()
        {
            Assert.That(Maybe.NotNull((string)null) == Maybe<string>.NoValue);
        }

        [Test]
        public void NotNull_WithNullValueType_ReturnsNoValue()
        {
            Assert.That(Maybe.NotNull((int?)null) == Maybe<int>.NoValue);
        }

        [Test]
        public void NotNull_WithNonNullReferenceType_ReturnsValue()
        {
            var value = Maybe.NotNull("Hello World!");

            Assert.IsTrue(value.HasValue);
            Assert.AreEqual("Hello World!", value.Value);
        }

        [Test]
        public void NotNull_WithNonNullValueType_ReturnsValue()
        {
            var value = Maybe.NotNull((int?) 42);

            Assert.IsTrue(value.HasValue);
            Assert.AreEqual(42, value.Value);
        }

        [Test]
        public void Coalesce_WithNonNullReferenceType_ReturnsValue()
        {
            var value = Maybe.Return("Hello World!")
                .Coalesce(x => x);

            Assert.IsTrue(value.HasValue);
            Assert.AreEqual("Hello World!", value.Value);
        }

        [Test]
        public void Coalesce_WithNonNullValueType_ReturnsValue()
        {
            var value = Maybe.Return((int?)42)
                .Coalesce(x => x);

            Assert.IsTrue(value.HasValue);
            Assert.AreEqual(42, value.Value);
        }

        [Test]
        public void Coalesce_WithNullReferenceType_ReturnsNoValue()
        {
            var value = Maybe.Return("Hello World!")
                .Coalesce(x => (string)null);

            Assert.That(value == Maybe<string>.NoValue);
        }

        [Test]
        public void Coalesce_WithNullValueType_ReturnsNoValue()
        {
            var value = Maybe.Return(42)
                .Coalesce(x => (int?)null);

            Assert.That(value == Maybe<int>.NoValue);
        }

        [Test]
        public void Return_ReturnsValueWrapedInMaybe()
        {
            var value = Maybe.Return("Hello World!");

            Assert.IsTrue(value.HasValue);
            Assert.AreEqual("Hello World!", value.Value);

            value = Maybe.Return((string)null);

            Assert.IsTrue(value.HasValue);
            Assert.IsNull(value.Value);
        }

        [Test]
        public void Select_ReturnsSelectedValueInMaybe()
        {
            string rawValue = "Hello World!";

            var value = Maybe
                .Return(rawValue)
                .Select(x => x.Length);

            Assert.IsTrue(value.HasValue);
            Assert.AreEqual(rawValue.Length, value.Value);
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
            var value = Maybe.NotNull(rawValue)
                .ValueOrDefault("{default}");

            Assert.AreEqual(rawValue, value);

            value = Maybe<string>.NoValue
                .ValueOrDefault("{default}");

            Assert.AreEqual("{default}", value);
        }

        [Test]
        public void Extract_ThrowsContainedException()
        {
            Assert.Throws<InvalidOperationException>(() =>
                new Maybe<int>(new InvalidOperationException())
                    .Extract());
        }

        [Test]
        public void Extract_DoesntSuppressException()
        {
            Assert.Throws<InvalidOperationException>(() =>
                Maybe.Throw<int>(new InvalidOperationException())
                    .Extract());
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
                .Select(x => 7/x)
                .OnException(x => executed = true);

            Assert.Throws<DivideByZeroException>(() => value.ValueOrDefault());
            Assert.IsTrue(executed);
        }

        [Test]
        public void OnException_ExecutesActionOnContainedException()
        {
            bool executed = false;

            var value = new Maybe<int>(new InvalidOperationException())
            .OnException(x => executed = true)
                .Run();

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
        public void OnException_OnContainedException_DoesNotExecuteHandlerAgainForExceptionCausedByHandler()
        {
            int count = 0;

            var value = new Maybe<int>(new InvalidOperationException())
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
                var x = new Maybe<int>(new InvalidOperationException())
                    .Or(42)
                    .Value;
            });
        }

        [Test]
        public void Suppress_ContinuesWithNoValue()
        {
            var value = Maybe<string>.Default
                .SelectMaybe(x => new Maybe<string>(new InvalidOperationException()))
                .Select(x => x.Length)
                .Where(x => x > 10)
                .Suppress();

            Assert.That(value == Maybe<int>.NoValue);
        }

        [Test]
        public void Suppress_ContinuesWithNewValue()
        {
            var value = Maybe<string>.Default
                .SelectMaybe(x => new Maybe<string>(new InvalidOperationException()))
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
                .When("Hello", "World");

            Assert.IsTrue(value.HasValue);
            Assert.AreEqual("World", value.Value);
        }

        [Test]
        public void When_PredicateIsFalse_UsesOriginalValue()
        {
            var value = Maybe.Return("Hello")
                .When("Goodbye", "World");

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
        public void NotNull_WithReferenceType_DefersExecutionUntilEvaluated()
        {
            bool executed = false;
            var value = Maybe.NotNull(() => { executed = true; return "42"; });

            Assert.IsFalse(executed);

            Assert.AreEqual("42", value.Value);
            Assert.IsTrue(executed);
        }

        [Test]
        public void NotNull_WithValueType_DefersExecutionUntilEvaluated()
        {
            bool executed = false;
            var value = Maybe.NotNull(() => { executed = true; return (int?)42; });

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
        public void ThrowOnException_DefersExecutionUntilEvaluated()
        {
            bool executed = false;
            var value = Maybe.Defer(() => { executed = true; return new Maybe<int>(new InvalidOperationException()); })
                .ThrowOnException();

            Assert.IsFalse(executed);

            Assert.Throws<InvalidOperationException>(() => { var notAssigned = value.HasValue; });
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
        public void Catch_ContainsThrownExceptions()
        {
            var value = Maybe<int>.Default
                .Select(x => 7/x)
                .Catch()
                .Run();

            Assert.IsNotNull(value.Exception);
            Assert.IsInstanceOf<DivideByZeroException>(value.Exception);
            Assert.IsFalse(value.HasValue);
        }

        [Test]
        public void Catch_DoesntCatchIfPredicateReturnsFalse()
        {
            var value = Maybe<int>.Default
                .Select(x => 7/x)
                .Catch(x => false);

            Assert.Throws<DivideByZeroException>(() => value.Run());
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

            Assert.Throws<InvalidCastException>(() => value.ValueOrDefault());
        }

        [Test]
        public void Cast_ThrowsException_PropigatesExistingException()
        {
            object foo = new List<string>();

            var value = new Maybe<List<string>>(new InvalidOperationException())
                .Cast<DateTime>();

            Assert.Throws<InvalidOperationException>(() => value.ValueOrDefault());
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
        public void OfType_PropigatesExistingException()
        {
            var value = new Maybe<ICollection<string>>(new InvalidOperationException())
                .OfType<DateTime>();

            Assert.Throws<InvalidOperationException>(() => value.ValueOrDefault());
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
            Assert.IsNull(value.Exception);
        }

        [Test]
        public void Or_YieldsExceptionIfFirstValueHasException()
        {
            var value = new Maybe<int>(new InvalidOperationException())
                .Or(Maybe.Return(42));

            Assert.Throws<InvalidOperationException>(() => value.ValueOrDefault());
        }

        [Test]
        public void Or_YieldsExceptionIfSecondValueHasExceptionAndFirstHasNoValue()
        {
            var value = Maybe<int>.NoValue
                .Or(new Maybe<int>(new InvalidOperationException()));

            Assert.Throws<InvalidOperationException>(() => value.ValueOrDefault());
        }

        [Test]
        public void Or_YieldsExceptionFromFirstValueIgnoringExceptionFromSecondValue()
        {
            var value = new Maybe<int>(new InvalidOperationException())
                .Or(new Maybe<int>(new NotSupportedException()));

            Assert.Throws<InvalidOperationException>(() => value.ValueOrDefault());
        }

        [Test]
        public void Or_YieldsExceptionFromFirstValueAndDoesNotEvaluateSecondValue()
        {
            bool executed = false;

            var value = new Maybe<int>(new InvalidOperationException())
                .Or(() => { executed = true; return 42; });

            Assert.Throws<InvalidOperationException>(() => value.ValueOrDefault());
            Assert.IsFalse(executed);
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
        public void Join_YieldsExceptionIfFirstValueHasException()
        {
            var value = new Maybe<int>(new InvalidOperationException())
                .Join(Maybe.Return(42));

            Assert.Throws<InvalidOperationException>(() => value.ValueOrDefault());
        }

        [Test]
        public void Join_YieldsExceptionIfSecondValueHasExceptionAndFirstHasValue()
        {
            var value = Maybe.Return(1)
                .Join(new Maybe<int>(new InvalidOperationException()));

            Assert.Throws<InvalidOperationException>(() => value.ValueOrDefault());
        }

        [Test]
        public void Join_YieldsExceptionFromFirstValueIgnoringExceptionFromSecondValue()
        {
            var value = new Maybe<int>(new InvalidOperationException())
                .Join(new Maybe<int>(new NotSupportedException()));

            Assert.Throws<InvalidOperationException>(() => value.ValueOrDefault());
        }

        [Test]
        public void Join_YieldsExceptionFromFirstValueAndDoesNotEvaluateSecondValue()
        {
            bool executed = false;

            var value = new Maybe<int>(new InvalidOperationException())
                .Join(Maybe.Defer(() => { executed = true; return 42; }));

            Assert.Throws<InvalidOperationException>(() => value.ValueOrDefault());
            Assert.IsFalse(executed);
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
        public void ToNullable_WithException_ThrowsException()
        {
            var value = new Maybe<int>(new InvalidOperationException());

            Assert.Throws<InvalidOperationException>(() => value.ToNullable());
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
        }

        [Test]
        public void If_ReturnsThenValue_IfBoolIsTrue()
        {
            var value = Maybe.If(true, 42.ToMaybe());

            Assert.That(value == 42);
        }

        [Test]
        public void If_ReturnsElseValue_IfBoolIsFalse()
        {
            var value = Maybe.If(false, 42.ToMaybe(), 7.ToMaybe());

            Assert.That(value == 7);
        }

        [Test]
        public void If_DefersExecutionUntilEvaluated_IfBoolIsTrue()
        {
            bool executed = false;
            var defered = Maybe.Defer(() => { executed = true; return "42"; });

            var value = Maybe.If(true, defered, defered);

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

            Assert.IsTrue(enumerable.SequenceEqual(new []{42}));
        }

        [Test]
        public void ToEnumerable_NoValue_YieldsEmptyStream()
        {
            var enumerable = Maybe<int>.NoValue
                .ToEnumerable();

            Assert.AreEqual(0, enumerable.Count());
        }

        [Test]
        public void ToEnumerable_WithException_ThrowsException()
        {
            var enumerable = new Maybe<int>(new InvalidOperationException())
                .ToEnumerable();

            Assert.Throws<InvalidOperationException>(() => enumerable.ToArray());
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

            Assert.IsNull(value.Exception);
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

            Assert.IsNull(value.Exception);
            Assert.IsTrue(value.HasValue);
            Assert.AreEqual("42", value.Value);
        }
    }
}
