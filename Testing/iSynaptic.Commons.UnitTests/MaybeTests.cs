using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            Assert.IsTrue(new Maybe<string>(null).HasValue);
        }

        [Test]
        public void AccessingValueOnAValueReturnsExpectedValue()
        {
            Assert.IsNull(new Maybe<string>(null).Value);
            Assert.AreEqual("Hello, World!", new Maybe<string>("Hello, World!").Value);
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
            Assert.IsTrue(new Maybe<string>(null) != Maybe<string>.NoValue);
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
                .Bind(x => 7/x)
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
                .Bind(x => 7/x);

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
            var value = Maybe.NotNull((int?)42);

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
                .NotNull(rawValue)
                .Return("{default}");

            Assert.AreEqual(rawValue, value);

            value = Maybe<string>.NoValue
                .Return("{default}");

            Assert.AreEqual("{default}", value);
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

        private static int ThrowsException(int x)
        {
            throw new InvalidOperationException();
        }
    }
}
 