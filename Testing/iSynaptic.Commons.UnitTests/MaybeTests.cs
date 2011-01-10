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
        public void GetHashCodeReturnsZeroForNoValue()
        {
            Assert.AreEqual(0, Maybe<int>.NoValue.GetHashCode());
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

        private static int ThrowsException(int x)
        {
            throw new InvalidOperationException();
        }
    }
}
 