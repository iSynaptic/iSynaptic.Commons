using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
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
        public void Bind_ThatReturnsScalar_ValueReturnsCorrectly()
        {
            var results = Maybe<int>.Default
                .Bind(x => x + 7)
                .Bind(x => x * 6);

            Assert.AreEqual(42, results.Value);
        }

        [Test]
        public void Bind_ThatReturnsMaybe_ValueReturnsCorrectly()
        {
            var results = Maybe<int>.Default
                .Bind(x => new Maybe<int>(x + 7))
                .Bind(x => new Maybe<int>(x * 6));

            Assert.AreEqual(42, results.Value);
        }

        [Test]
        public void Bind_ThatImplicitlyThrowsException_ValueRethrowsException()
        {
            var results = Maybe<int>
                .Default
                .Bind(x => 7 / x);

            Assert.Throws<DivideByZeroException>(() => { var x = results.Value; });
        }

        [Test]
        public void Bind_ThatImplicitlyThrowsException_ExceptionReturnsCorrectly()
        {
            var results = Maybe<int>
                .Default
                .Bind(x => 7 / x);

            Assert.IsInstanceOf<DivideByZeroException>(results.Exception);
        }

        [Test]
        public void Bind_WhenExceptionOccurs_DoesNotExecuteRemainingComputations()
        {
            bool executed = false;

            var results = Maybe<int>
                .Default
                .Bind(x => 7 / x)
                .Bind(x => executed = true);

            Assert.Throws<DivideByZeroException>(() => { var x = results.Value; });
            Assert.IsFalse(executed);
        }

        [Test]
        public void Bind_WhenNoValue_DoesNotExecuteRemaningComputations()
        {
            bool executed = false;

            var results = Maybe<int>
                .Default
                .Bind(x => x + 7)
                .Bind(x => Maybe<int>.NoValue)
                .Bind(x => executed = true);

            Assert.IsFalse(results.HasValue);
            Assert.IsFalse(executed);
        }

        [Test]
        public void Equals_WithSameException_ReturnsTrue()
        {
            var results = Maybe<int>
                .Default
                .Bind(x => 7 / x);

            Assert.IsTrue(results.Equals(results));
        }

        [Test]
        public void Equals_WithDifferentException_ReturnsFalse()
        {
            var first = Maybe<int>
                .Default
                .Bind(x => 7 / x);

            var second = Maybe<int>
                .Default
                .Bind(ThrowsException);

            Assert.IsFalse(first.Equals(second));
        }

        [Test]
        public void Equals_WithOnlyOneException_ReturnsFalse()
        {
            var result = Maybe<int>
                .Default
                .Bind(x => 7 / x);

            Assert.IsFalse(Maybe<int>.Default.Equals(result));

        }

        [Test]
        public void GetHashCode_WithException_ReturnsExceptionsHashCode()
        {
            var result = Maybe<int>
                .Default
                .Bind(x => 7 / x);

            Assert.AreEqual(result.Exception.GetHashCode(), result.GetHashCode());
        }

        [Test]
        public void NotNull_WithNullReferenceType_ReturnsNoValue()
        {
            Assert.IsFalse(Maybe.Value((string)null).NotNull().HasValue);
        }

        [Test]
        public void NotNull_WithNullValueType_ReturnsNoValue()
        {
            Assert.IsFalse(Maybe.Value((int?)null).NotNull().HasValue);
        }

        [Test]
        public void NotNull_WithNonNullReferenceType_ReturnsValue()
        {
            var value = Maybe.Value("Hello World!")
                .NotNull();

            Assert.IsTrue(value.HasValue);
            Assert.AreEqual("Hello World!", value.Value);
        }

        [Test]
        public void NotNull_WithNonNullValueType_ReturnsValue()
        {
            var value = Maybe.Value((int?)42)
                .NotNull();

            Assert.IsTrue(value.HasValue);
            Assert.AreEqual(42, value.Value);
        }

        [Test]
        public void Value_ReturnsValueWrapedInMaybe()
        {
            var value = Maybe.Value("Hello World!");

            Assert.IsTrue(value.HasValue);
            Assert.AreEqual("Hello World!", value.Value);

            value = Maybe.Value((string)null);

            Assert.IsTrue(value.HasValue);
            Assert.IsNull(value.Value);
        }

        [Test]
        public void Select_ReturnsSelectedValueInMaybe()
        {
            string rawValue = "Hello World!";

            var value = Maybe
                .Value(rawValue)
                .Select(x => x.Length);

            Assert.IsTrue(value.HasValue);
            Assert.AreEqual(rawValue.Length, value.Value);
        }

        [Test]
        public void Where_ReturnsValueIfPredicateReturnsTrue()
        {
            string rawValue = "Hello World!";

            var value = Maybe
                .Value(rawValue)
                .Where(x => x.Length == rawValue.Length);

            Assert.IsTrue(value.HasValue);
            Assert.AreEqual(rawValue, value.Value);

            value = Maybe
                .Value(rawValue)
                .Where(x => x.Length == rawValue.Length - 1);

            Assert.IsFalse(value.HasValue);
        }

        [Test]
        public void Unless_ReturnsValueIfPredicateReturnsFalse()
        {
            string rawValue = "Hello World!";

            var value = Maybe
                .Value(rawValue)
                .Unless(x => x.Length == rawValue.Length - 1);

            Assert.IsTrue(value.HasValue);
            Assert.AreEqual(rawValue, value.Value);

            value = Maybe
                .Value(rawValue)
                .Unless(x => x.Length == rawValue.Length);

            Assert.IsFalse(value.HasValue);
        }

        [Test]
        public void Return_UnwrapsValueIfItHasAvalue()
        {
            var rawValue = "Hello World!";
            var value = Maybe
                .Value(rawValue)
                .NotNull()
                .Return("{default}");

            Assert.AreEqual(rawValue, value);

            value = Maybe<string>.NoValue
                .Return("{default}");

            Assert.AreEqual("{default}", value);
        }

        [Test]
        public void Return_RethrowsExistingException()
        {
            Assert.Throws<InvalidOperationException>(() =>
                Maybe.Value<int>(() => { throw new InvalidOperationException(); })
                    .Return(42));
        }

        [Test]
        public void Return_WhereExceptionExists_CanBeAbsorbedByOnException()
        {
            var value = Maybe.Value<int>(() => { throw new InvalidOperationException(); })
                .OnException(27)
                .Return(42);

            Assert.AreEqual(27, value);
        }

        [Test]
        public void Do_CallsActionIfHasValueIsTrue()
        {
            bool didExecute = false;

            var value = Maybe<string>.NoValue
                .Do(x => didExecute = true);

            Assert.IsFalse(value.HasValue);
            Assert.IsFalse(didExecute);

            value = Maybe.Value("Hello World!")
                .Do(x => didExecute = true);

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

            Maybe.Value(rawValue)
                .Assign(ref refString)
                .Select(x => x.Length)
                .Assign(ref refInt);

            Assert.AreEqual(rawValue, refString);
            Assert.AreEqual(rawValue.Length, refInt);
        }

        [Test]
        public void OnNoValue_ContinuesWithNewValue()
        {
            var value = Maybe<int>.NoValue
                .OnNoValue(42)
                .Value;

            Assert.AreEqual(42, value);
        }

        [Test]
        public void OnNoValue_DoesNotHandleExceptions()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                var x = new Maybe<int>(new InvalidOperationException())
                    .OnNoValue(42)
                    .Value;
            });
        }

        [Test]
        public void OnException_ContinuesWithNewValue()
        {
            Func<string, string> throwException = x =>
                { throw new InvalidOperationException(); };

            var value = Maybe<string>.Default
                .Bind(throwException)
                .Select(x => x.Length)
                .Where(x => x > 10)
                .OnException(42);

            Assert.IsTrue(value.HasValue);
            Assert.AreEqual(42, value.Value);
        }

        [Test]
        public void OnException_WhenHandlerThrowsException_HandlerExceptionPropagates()
        {
            Func<string, string> throwException = x =>
                { throw new InvalidOperationException(); };

            Func<Exception, Maybe<int>> handler = x =>
                { throw new NullReferenceException(); };

            var value = Maybe<string>.Default
                .Bind(throwException)
                .Select(x => x.Length)
                .Where(x => x > 10)
                .OnException(handler);

            Assert.IsInstanceOf<NullReferenceException>(value.Exception);
        }

        [Test]
        public void With_WhenHasValue_Executes()
        {
            int checkValue = 0;

            var value = Maybe.Value("Hello")
                .With(x => x.Length, x => checkValue = x)
                .Return();

            Assert.AreEqual("Hello", value);
            Assert.AreEqual(5, checkValue);
        }

        [Test]
        public void When_PredicateIsTrue_UsesComputation()
        {
            var value = Maybe.Value("Hello")
                .When("Hello", "World");

            Assert.IsTrue(value.HasValue);
            Assert.AreEqual("World", value.Value);
        }

        [Test]
        public void When_PredicateIsFalse_UsesOriginalValue()
        {
            var value = Maybe.Value("Hello")
                .When("Goodbye", "World");

            Assert.IsTrue(value.HasValue);
            Assert.AreEqual("Hello", value.Value);
        }

        [Test]
        public void When_WithAction_ExecutesAction()
        {
            string output = null;

            var value = Maybe.Value("Hello")
                .When("Hello", (Action<string>)(x => output = x))
                .Run();

            Assert.AreEqual("Hello", output);
        }

        [Test]
        public void Value_DefersExecutionUntilEvaluated()
        {
            bool executed = false;
            var value = Maybe.Value(() => { executed = true; return 42; });

            Assert.IsFalse(executed);

            Assert.AreEqual(42, value.Value);
            Assert.IsTrue(executed);
        }

        [Test]
        public void NotNull_WithReferenceType_DefersExecutionUntilEvaluated()
        {
            bool executed = false;
            var value = Maybe.Value(() => { executed = true; return "42"; }).NotNull();

            Assert.IsFalse(executed);

            Assert.AreEqual("42", value.Value);
            Assert.IsTrue(executed);
        }

        [Test]
        public void NotNull_WithValueType_DefersExecutionUntilEvaluated()
        {
            bool executed = false;
            var value = Maybe.Value(() => { executed = true; return (int?)42; }).NotNull();

            Assert.IsFalse(executed);

            Assert.AreEqual(42, value.Value);
            Assert.IsTrue(executed);
        }

        [Test]
        public void Select_DefersExecutionUntilEvaluated()
        {
            bool executed = false;
            var value = Maybe.Value(() => { executed = true; return "42"; })
                .Select(x => x.Length);

            Assert.IsFalse(executed);

            Assert.AreEqual(2, value.Value);
            Assert.IsTrue(executed);
        }

        [Test]
        public void Select_WhenSelectorReturnsMaybe_DefersExecutionUntilEvaluated()
        {
            bool executed = false;
            var value = Maybe.Value(() => { executed = true; return "42"; })
                .Select(x => Maybe<int>.NoValue);

            Assert.IsFalse(executed);

            Assert.IsFalse(value.HasValue);
            Assert.IsTrue(executed);
        }

        [Test]
        public void Synchronize_PreventsMultipleEvaluation()
        {
            int count = 0;
            var value = Maybe.Value(() => { count++; Thread.Sleep(250); return "42"; })
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
            var value = Maybe.Value(() => { executed = true; return "42"; })
                .Where(x => x.Length == 2);

            Assert.IsFalse(executed);

            Assert.AreEqual("42", value.Value);
            Assert.IsTrue(executed);
        }

        [Test]
        public void Unless_DefersExecutionUntilEvaluated()
        {
            bool executed = false;
            var value = Maybe.Value(() => { executed = true; return "42"; })
                .Unless(x => x.Length == 2);

            Assert.IsFalse(executed);

            Assert.IsFalse(value.HasValue);
            Assert.IsTrue(executed);
        }

        [Test]
        public void Do_DefersExecutionUntilEvaluated()
        {
            bool executed = false;
            var value = Maybe.Value(() => { executed = true; return "42"; })
                .Do(x => executed = true);

            Assert.IsFalse(executed);

            Assert.AreEqual("42", value.Value);
            Assert.IsTrue(executed);
        }

        [Test]
        public void Assign_ExecutesImmediately()
        {
            bool executed = false;
            Maybe.Value(true)
                .Assign(ref executed);

            Assert.IsTrue(executed);
        }

        [Test]
        public void OnException_DefersExecutionUntilEvaluated()
        {
            bool executed = false;
            var value = Maybe.Value(() => { executed = true; return "42"; })
                .OnException("Hello")
                .OnException(x => executed = true);

            Assert.IsFalse(executed);

            Assert.AreEqual("42", value.Value);
            Assert.IsTrue(executed);
        }

        [Test]
        public void ThrowOnException_DefersExecutionUntilEvaluated()
        {
            bool executed = false;
            var value = Maybe.Value<int>(() => { executed = true; throw new InvalidOperationException(); })
                .ThrowOnException();

            Assert.IsFalse(executed);

            Assert.Throws<InvalidOperationException>(() => { var notAssigned = value.Value; });
            Assert.IsTrue(executed);
        }

        [Test]
        public void ThrowOnNoValue_DefersExecutionUntilEvaluated()
        {
            bool executed = false;
            var value = Maybe.Value(() => { executed = true; return 42; })
                .Select(x => Maybe<int>.NoValue)
                .ThrowOnNoValue(new InvalidOperationException());

            Assert.IsFalse(executed);

            Assert.Throws<InvalidOperationException>(() => { var notAssigned = value.Value; });
            Assert.IsTrue(executed);
        }

        [Test]
        public void Synchronize_DeferesExecutionUntilEvaluated()
        {
            bool executed = false;
            var value = Maybe.Value(() => { executed = true; return 42; })
                .Synchronize();

            Assert.IsFalse(executed);

            Assert.AreEqual(42, value.Value);
            Assert.IsTrue(executed);
        }

        [Test]
        public void Cast_ReturnsCastedType()
        {
            ICollection<string> foo = new List<string>();

            ICollection<string> value = Maybe.Value<object>(foo)
                .Cast<object, ICollection<string>>()
                .Value;

            Assert.IsTrue(ReferenceEquals(foo, value));
        }

        [Test]
        public void Cast_ReturnsNoValue_WhenCastIsNotPossible()
        {
            ICollection<string> foo = new List<string>();

            var value = Maybe.Value<object>(foo)
                .Cast<object, DateTime>();

            Assert.IsFalse(value.HasValue);
        }

        [Test]
        public void Cast_DeferesExecutionUntilEvaluated()
        {
            ICollection<string> foo = new List<string>();

            bool executed = false;
            var value = Maybe.Value<object>(() => { executed = true; return foo; })
                .Cast<object, ICollection<string>>();

            Assert.IsFalse(executed);

            Assert.IsTrue(ReferenceEquals(foo, value.Value));
            Assert.IsTrue(executed);
        }

        [Test]
        public void Or_ReturnsFirstValueIfHasValue()
        {
            var value = Maybe.Value(1)
                .Or(Maybe.Value(42))
                .Return();

            Assert.AreEqual(1, value);
        }

        [Test]
        public void Or_ReturnsSecondValueIfFirstDoesNotHaveValue()
        {
            var value = Maybe<int>.NoValue
                .Or(Maybe.Value(42))
                .Return();

            Assert.AreEqual(42, value);
        }

        [Test]
        public void Or_YieldsExceptionIfFirstValueHasException()
        {
            var value = new Maybe<int>(new InvalidOperationException())
                .Or(Maybe.Value(42));

            Assert.Throws<InvalidOperationException>(() => value.Return());
        }

        [Test]
        public void Or_YieldsExceptionIfSecondValueHasExceptionAndFirstHasNoValue()
        {
            var value = Maybe<int>.NoValue
                .Or(new Maybe<int>(new InvalidOperationException()));

            Assert.Throws<InvalidOperationException>(() => value.Return());
        }

        [Test]
        public void Or_YieldsExceptionFromFirstValueIgnoringExceptionFromSecondValue()
        {
            var value = new Maybe<int>(new InvalidOperationException())
                .Or(new Maybe<int>(new NotSupportedException()));

            Assert.Throws<InvalidOperationException>(() => value.Return());
        }

        [Test]
        public void Or_YieldsExceptionFromFirstValueAndDoesNotEvaluateSecondValue()
        {
            bool executed = false;

            var value = new Maybe<int>(new InvalidOperationException())
                .Or(Maybe.Value(() => { executed = true; return 42; }));

            Assert.Throws<InvalidOperationException>(() => value.Return());
            Assert.IsFalse(executed);
        }

        [Test]
        public void Join_ReturnsBothValueIfHasValue()
        {
            var value = Maybe.Value(1)
                .Join(Maybe.Value(42))
                .Return();

            Assert.AreEqual(Tuple.Create(1, 42), value);
        }

        [Test]
        public void Join_ReturnsNoValueIfFirstDoesNotHaveValue()
        {
            var value = Maybe<int>.NoValue
                .Join(Maybe.Value(42));
                
            Assert.IsFalse(value.HasValue);
        }

        [Test]
        public void Join_YieldsExceptionIfFirstValueHasException()
        {
            var value = new Maybe<int>(new InvalidOperationException())
                .Join(Maybe.Value(42));

            Assert.Throws<InvalidOperationException>(() => value.Return());
        }

        [Test]
        public void Join_YieldsExceptionIfSecondValueHasExceptionAndFirstHasValue()
        {
            var value = Maybe.Value(1)
                .Join(new Maybe<int>(new InvalidOperationException()));

            Assert.Throws<InvalidOperationException>(() => value.Return());
        }

        [Test]
        public void Join_YieldsExceptionFromFirstValueIgnoringExceptionFromSecondValue()
        {
            var value = new Maybe<int>(new InvalidOperationException())
                .Join(new Maybe<int>(new NotSupportedException()));

            Assert.Throws<InvalidOperationException>(() => value.Return());
        }

        [Test]
        public void Join_YieldsExceptionFromFirstValueAndDoesNotEvaluateSecondValue()
        {
            bool executed = false;

            var value = new Maybe<int>(new InvalidOperationException())
                .Join(Maybe.Value(() => { executed = true; return 42; }));

            Assert.Throws<InvalidOperationException>(() => value.Return());
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
            var value = Maybe.Value(42)
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

            var waitEvent = new ManualResetEventSlim();
            var value = Maybe.Value(42)
                .Do(x => started = 1)
                .Select(x =>
                        {
                            waitEvent.Wait();
                            return x;
                        })
                .Do(x => ended = 1)
                .RunAsync();

            var waitForStarted = Task.Factory.StartNew(() => { while (Thread.VolatileRead(ref started) != 1) continue; });
            Assert.IsTrue(waitForStarted.Wait(TimeSpan.FromSeconds(0.5)), "Wait for started failed.");

            Assert.AreEqual(0, ended);
            waitEvent.Set();

            var waitForEnded = Task.Factory.StartNew(() => { while (Thread.VolatileRead(ref ended) != 1) continue; });
            Assert.IsTrue(waitForEnded.Wait(TimeSpan.FromSeconds(0.5)), "Wait for ended failed.");

            Assert.AreEqual(42, value.Value);
        }

        private static int ThrowsException(int x)
        {
            throw new InvalidOperationException();
        }
    }
}
