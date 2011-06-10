using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace iSynaptic.Commons
{
    [TestFixture]
    public class MaybeTests
    {
        [Test]
        public void AccessingValueOnNoValueThrowsException()
        {
            Assert.Throws<InvalidOperationException>(() => Console.WriteLine(Maybe<string>.NoValue.Value));
        }

        [Test]
        public void HasValueOnNoValueReturnsFalse()
        {
            Assert.IsFalse(Maybe<string>.NoValue.HasValue);
        }

        [Test]
        public void HasValueOnAValueReturnsTrue()
        {
            Assert.IsTrue(new Maybe<string>("").HasValue);
        }

        [Test]
        public void AccessingValueOnAValueReturnsExpectedValue()
        {
            Assert.IsNull(new Maybe<string>((string)null).Value);
            Assert.AreEqual("Hello, World!", new Maybe<string>("Hello, World!").Value);
        }

        [Test]
        public void AccessingValueOnAnException_ThrowsException()
        {
            var value = new Maybe<string>(new InvalidOperationException("42"));

            var exception = Assert.Throws<InvalidOperationException>(() => { var x = value.Value; });
            Assert.AreEqual("42", exception.Message);
        }

        [Test]
        public void DefaultValueHasNoValue()
        {
            Maybe<int> val = default(Maybe<int>);
            Assert.IsFalse(val.HasValue);
        }

        [Test]
        public void DefaultValueEqualsNoValue()
        {
            Maybe<int> val = default(Maybe<int>);
            Assert.IsTrue(val == Maybe<int>.NoValue);
        }

        [Test]
        public void NullValueDoesNotEqualNoValue()
        {
            Assert.IsTrue(new Maybe<string>((string)null) != Maybe<string>.NoValue);
        }

        [Test]
        public void TwoMaybesWithSameValueAreEqual()
        {
            var left = new Maybe<int>(7);
            var right = new Maybe<int>(7);

            Assert.IsTrue(left == right);
        }

        [Test]
        public void EqualsUsesNonGenericEqualsOfUnderlyingValueIfGenericIsNotAvailable()
        {
            var left = new Maybe<DayOfWeek>(DayOfWeek.Friday);
            var right = new Maybe<DayOfWeek>(DayOfWeek.Friday);

            Assert.IsTrue(left == right);
        }

        [Test]
        public void BoxedMaybeEqualsBehavesCorrectly()
        {
            object left = new Maybe<int>(7);
            object right = new Maybe<int>(7);

            Assert.IsTrue(left.Equals(right));
            Assert.IsFalse(left.Equals(null));
            Assert.IsFalse(left.Equals(7));
        }

        [Test]
        public void GetHashCodeReturnsNegativeOneForNoValue()
        {
            Assert.AreEqual(-1, Maybe<int>.NoValue.GetHashCode());
        }

        [Test]
        public void GetHashCodeReturnsUnderlyingHashCodeWhenHasValue()
        {
            int val = 42;
            Assert.AreEqual(val.GetHashCode(), new Maybe<int>(val).GetHashCode());
        }

        [Test]
        public void EvaluationOfMaybeOnlyOccursOnce()
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
        }

        [Test]
        public void Select_ThatReturnsScalar_ValueReturnsCorrectly()
        {
            var results = Maybe<int>.Default
                .Select(x => x + 7)
                .Select(x => x * 6);

            Assert.AreEqual(42, results.Value);
        }

        [Test]
        public void SelectMaybe_ThatReturnsMaybe_ValueReturnsCorrectly()
        {
            var results = Maybe<int>.Default
                .SelectMaybe(x => (x + 7).ToMaybe())
                .SelectMaybe(x => (x * 6).ToMaybe());

            Assert.AreEqual(42, results.Value);
        }

        [Test]
        public void Select_ThatImplicitlyThrowsException_ValueRethrowsException()
        {
            var results = Maybe<int>
                .Default
                .Select(x => (7 / x).ToMaybe());

            Assert.Throws<DivideByZeroException>(() => { var x = results.Value; });
        }

        [Test]
        public void Select_ThatImplicitlyThrowsException_BubblesUpException()
        {
            var results = Maybe<int>
                .Default
                .Select(x => (7 / x).ToMaybe());

            Assert.Throws<DivideByZeroException>(() => results.Run());
        }

        [Test]
        public void Select_WhenExceptionOccurs_DoesNotExecuteRemainingComputations()
        {
            bool executed = false;

            var results = Maybe<int>
                .Default
                .Select(x => (7 / x).ToMaybe())
                .Select(x => (executed = true).ToMaybe());

            Assert.Throws<DivideByZeroException>(() => { var x = results.Value; });
            Assert.IsFalse(executed);
        }

        [Test]
        public void Select_WhenNoValue_DoesNotExecuteRemaningComputations()
        {
            bool executed = false;

            Maybe<bool> results = Maybe<int>
                .Default
                .SelectMaybe(x => (x + 7).ToMaybe())
                .SelectMaybe(x => Maybe<int>.NoValue)
                .SelectMaybe(x => (executed = true).ToMaybe());

            Assert.IsFalse(results.HasValue);
            Assert.IsFalse(executed);
        }

        [Test]
        public void ComprehensionSyntaxIsWorking()
        {
            Maybe<int> value = 
                        from x in Maybe.Return(6)
                        from y in Maybe.Return(7)
                        let ultimateAnswer = x*y
                        where ultimateAnswer == 42
                        select ultimateAnswer;

            Assert.IsTrue(value.HasValue);
            Assert.IsNull(value.Exception);
            Assert.AreEqual(42, value.Value);
        }

        [Test]
        public void Equals_WithSameException_ReturnsTrue()
        {
            var exception = new DivideByZeroException();

            var first = new Maybe<int>(exception);
            var second = new Maybe<int>(exception);

            Assert.IsTrue(first.Equals(second));
        }

        [Test]
        public void Equals_WithDifferentException_ReturnsFalse()
        {
            var first = new Maybe<int>(new DivideByZeroException());
            var second = new Maybe<int>(new DivideByZeroException());

            Assert.IsFalse(first.Equals(second));
        }

        [Test]
        public void Equals_WithOnlyOneException_ReturnsFalse()
        {
            var result = new Maybe<int>(new DivideByZeroException());
            Assert.IsFalse(Maybe<int>.Default.Equals(result));
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
            Assert.IsFalse(Maybe.NotNull((string)null).HasValue);
        }

        [Test]
        public void NotNull_WithNullValueType_ReturnsNoValue()
        {
            Assert.IsFalse(Maybe.NotNull((int?)null).HasValue);
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

            Assert.IsFalse(value.HasValue);
        }

        [Test]
        public void Coalesce_WithNullValueType_ReturnsNoValue()
        {
            var value = Maybe.Return(42)
                .Coalesce(x => (int?)null);

            Assert.IsFalse(value.HasValue);
        }

        [Test]
        public void Value_ReturnsValueWrapedInMaybe()
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
        public void Extract_UnwrapsValueIfItHasAvalue()
        {
            var rawValue = "Hello World!";
            var value = Maybe.NotNull(rawValue)
                .Extract("{default}");

            Assert.AreEqual(rawValue, value);

            value = Maybe<string>.NoValue
                .Extract("{default}");

            Assert.AreEqual("{default}", value);
        }

        [Test]
        public void Extract_RethrowsExistingException()
        {
            Assert.Throws<InvalidOperationException>(() =>
                Maybe.Return<int>(() => { throw new InvalidOperationException(); })
                    .Extract(42));
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
        public void Or_DoesNotHandleExceptions()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                var x = new Maybe<int>(new InvalidOperationException())
                    .Or(42)
                    .Value;
            });
        }

        [Test]
        public void SuppressException_ContinuesWithNewValue()
        {
            var value = Maybe<string>.Default
                .SelectMaybe(x => new Maybe<string>(new InvalidOperationException()))
                .Select(x => x.Length)
                .Where(x => x > 10)
                .SuppressException(42);

            Assert.IsTrue(value.HasValue);
            Assert.AreEqual(42, value.Value);
        }

        [Test]
        public void With_WhenHasValue_Executes()
        {
            int checkValue = 0;

            var value = Maybe.Return("Hello")
                .With(x => x.Length, x => checkValue = x)
                .Extract();

            Assert.AreEqual("Hello", value);
            Assert.AreEqual(5, checkValue);
        }

        [Test]
        public void With_WhenNoValue_DoesNotExecutes()
        {
            int checkValue = 0;

            var value = Maybe.Return("Hello")
                .With(x => Maybe<int>.NoValue, x => checkValue = x)
                .Extract();

            Assert.AreEqual("Hello", value);
            Assert.AreEqual(0, checkValue);
        }

        [Test]
        public void When_PredicateIsTrue_UsesComputation()
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

            var value = Maybe.Return("Hello")
                .When("Hello", x => output = x)
                .Run();

            Assert.AreEqual("Hello", output);
        }

        [Test]
        public void Return_DefersExecutionUntilEvaluated()
        {
            bool executed = false;
            var value = Maybe.Return(() => { executed = true; return 42; });

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
            var value = Maybe.Return(() => { executed = true; return "42"; })
                .Select(x => x.Length);

            Assert.IsFalse(executed);

            Assert.AreEqual(2, value.Value);
            Assert.IsTrue(executed);
        }

        [Test]
        public void Select_WhenSelectorReturnsMaybe_DefersExecutionUntilEvaluated()
        {
            bool executed = false;
            Maybe<int> value = Maybe.Return(() => { executed = true; return "42"; })
                .SelectMaybe(x => Maybe<int>.NoValue);

            Assert.IsFalse(executed);

            Assert.IsFalse(value.HasValue);
            Assert.IsTrue(executed);
        }

        [Test]
        public void Synchronize_PreventsMultipleEvaluation()
        {
            int count = 0;
            var value = Maybe.Return(() => { count++; Thread.Sleep(250); return "42"; })
                .Synchronize();

            string results = null;

            var t1 = Task.Factory.StartNew(() => results = value.Value);
            var t2 = Task.Factory.StartNew(() => results = value.Value);

            Task.WaitAll(t1, t2);

            Assert.AreEqual("42", results);
            Assert.AreEqual(1, count);
        }

        [Test]
        public void Where_DefersExecutionUntilEvaluated()
        {
            bool executed = false;
            var value = Maybe.Return(() => { executed = true; return "42"; })
                .Where(x => x.Length == 2);

            Assert.IsFalse(executed);

            Assert.AreEqual("42", value.Value);
            Assert.IsTrue(executed);
        }

        [Test]
        public void Unless_DefersExecutionUntilEvaluated()
        {
            bool executed = false;
            var value = Maybe.Return(() => { executed = true; return "42"; })
                .Unless(x => x.Length == 2);

            Assert.IsFalse(executed);

            Assert.IsFalse(value.HasValue);
            Assert.IsTrue(executed);
        }

        [Test]
        public void OnValue_DefersExecutionUntilEvaluated()
        {
            bool executed = false;
            var value = Maybe.Return(() => { executed = true; return "42"; })
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
        public void SuppressException_DefersExecutionUntilEvaluated()
        {
            bool executed = false;
            var value = Maybe.Return(() => { executed = true; return "42"; })
                .OnValue(x => executed = true)
                .SuppressException("Hello");

            Assert.IsFalse(executed);

            Assert.AreEqual("42", value.Value);
            Assert.IsTrue(executed);
        }

        [Test]
        public void ThrowOnException_DefersExecutionUntilEvaluated()
        {
            bool executed = false;
            var value = Maybe.Return<int>(() => { executed = true; throw new InvalidOperationException(); })
                .ThrowOnException();

            Assert.IsFalse(executed);

            Assert.Throws<InvalidOperationException>(() => { var notAssigned = value.Value; });
            Assert.IsTrue(executed);
        }

        [Test]
        public void ThrowOnNoValue_DefersExecutionUntilEvaluated()
        {
            bool executed = false;
            Maybe<int> value = Maybe.Return(() => { executed = true; return 42; })
                .SelectMaybe(x => Maybe<int>.NoValue)
                .ThrowOnNoValue(new InvalidOperationException());

            Assert.IsFalse(executed);

            Assert.Throws<InvalidOperationException>(() => { var notAssigned = value.Value; });
            Assert.IsTrue(executed);
        }

        [Test]
        public void Synchronize_DeferesExecutionUntilEvaluated()
        {
            bool executed = false;
            var value = Maybe.Return(() => { executed = true; return 42; })
                .Synchronize();

            Assert.IsFalse(executed);

            Assert.AreEqual(42, value.Value);
            Assert.IsTrue(executed);
        }

        [Test]
        public void CatchExceptions_ContainsThrownExceptions()
        {
            var value = Maybe<int>.Default
                .Select(x => 7/x)
                .CatchExceptions()
                .Run();

            Assert.IsNotNull(value.Exception);
            Assert.IsInstanceOf<DivideByZeroException>(value.Exception);
            Assert.IsFalse(value.HasValue);
        }

        [Test]
        public void Cast_ReturnsCastedType()
        {
            ICollection<string> foo = new List<string>();

            ICollection<string> value = Maybe.Return<object>(foo)
                .Cast<ICollection<string>>()
                .Value;

            Assert.IsTrue(ReferenceEquals(foo, value));
        }

        [Test]
        public void Cast_ThrowsException_WhenCastIsNotPossible()
        {
            ICollection<string> foo = new List<string>();

            var value = Maybe.Return<object>(foo)
                .Cast<DateTime>();

            Assert.Throws<InvalidCastException>(() => value.Extract());
        }

        [Test]
        public void Cast_DeferesExecutionUntilEvaluated()
        {
            ICollection<string> foo = new List<string>();

            bool executed = false;
            var value = Maybe.Return<object>(() => { executed = true; return foo; })
                .Cast<ICollection<string>>();

            Assert.IsFalse(executed);

            Assert.IsTrue(ReferenceEquals(foo, value.Value));
            Assert.IsTrue(executed);
        }

        [Test]
        public void Or_ReturnsFirstValueIfHasValue()
        {
            var value = Maybe.Return(1)
                .Or(Maybe.Return(42))
                .Extract();

            Assert.AreEqual(1, value);
        }

        [Test]
        public void Or_ReturnsSecondValueIfFirstDoesNotHaveValue()
        {
            var value = Maybe<int>.NoValue
                .Or(Maybe.Return(42))
                .Extract();

            Assert.AreEqual(42, value);
        }

        [Test]
        public void Or_ReturnsSecondNoValue_IfBothAreNoValue()
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

            Assert.Throws<InvalidOperationException>(() => value.Extract());
        }

        [Test]
        public void Or_YieldsExceptionIfSecondValueHasExceptionAndFirstHasNoValue()
        {
            var value = Maybe<int>.NoValue
                .Or(new Maybe<int>(new InvalidOperationException()));

            Assert.Throws<InvalidOperationException>(() => value.Extract());
        }

        [Test]
        public void Or_YieldsExceptionFromFirstValueIgnoringExceptionFromSecondValue()
        {
            var value = new Maybe<int>(new InvalidOperationException())
                .Or(new Maybe<int>(new NotSupportedException()));

            Assert.Throws<InvalidOperationException>(() => value.Extract());
        }

        [Test]
        public void Or_YieldsExceptionFromFirstValueAndDoesNotEvaluateSecondValue()
        {
            bool executed = false;

            var value = new Maybe<int>(new InvalidOperationException())
                .Or(Maybe.Return(() => { executed = true; return 42; }));

            Assert.Throws<InvalidOperationException>(() => value.Extract());
            Assert.IsFalse(executed);
        }

        [Test]
        public void Join_ReturnsBothValueIfHasValue()
        {
            var value = Maybe.Return(1)
                .Join(Maybe.Return(42))
                .Extract();

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

            Assert.Throws<InvalidOperationException>(() => value.Extract());
        }

        [Test]
        public void Join_YieldsExceptionIfSecondValueHasExceptionAndFirstHasValue()
        {
            var value = Maybe.Return(1)
                .Join(new Maybe<int>(new InvalidOperationException()));

            Assert.Throws<InvalidOperationException>(() => value.Extract());
        }

        [Test]
        public void Join_YieldsExceptionFromFirstValueIgnoringExceptionFromSecondValue()
        {
            var value = new Maybe<int>(new InvalidOperationException())
                .Join(new Maybe<int>(new NotSupportedException()));

            Assert.Throws<InvalidOperationException>(() => value.Extract());
        }

        [Test]
        public void Join_YieldsExceptionFromFirstValueAndDoesNotEvaluateSecondValue()
        {
            bool executed = false;

            var value = new Maybe<int>(new InvalidOperationException())
                .Join(Maybe.Return(() => { executed = true; return 42; }));

            Assert.Throws<InvalidOperationException>(() => value.Extract());
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
            var disposable = disposer.ToDisposable();

            Maybe
                .Using(disposable, x => Maybe<int>.NoValue)
                .Run();

            Assert.IsTrue(disposed);
        }

        [Test]
        public void Using_DefersExecution()
        {
            bool disposed = false;
            Action disposer = () => disposed = true;
            var disposable = disposer.ToDisposable();

            var value = Maybe
                .Using(disposable, x => Maybe<int>.NoValue);

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
    }
}
