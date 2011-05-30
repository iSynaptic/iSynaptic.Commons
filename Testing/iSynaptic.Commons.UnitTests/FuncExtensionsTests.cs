using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace iSynaptic.Commons
{
    [TestFixture]
    public partial class FuncExtensionsTests
    {
        [Test]
        public void MakeConditional()
        {
            Func<int, int> func = null;

            Assert.Throws<ArgumentNullException>(() => { func.MakeConditional(x => x < 3); });

            func = x => x;
            Assert.Throws<ArgumentNullException>(() => { func.MakeConditional(null); });

            var simpleConditionalFunc = func.MakeConditional(x => x > 5);
            Assert.AreEqual(0, simpleConditionalFunc(1));
            Assert.AreEqual(6, simpleConditionalFunc(6));

            var withDefaultValueFunc = func.MakeConditional(x => x > 5, -1);
            Assert.AreEqual(-1, withDefaultValueFunc(1));
            Assert.AreEqual(6, withDefaultValueFunc(6));

            var withFalseFunc = func.MakeConditional(x => x > 5, x => x * 2);
            Assert.AreEqual(2, withFalseFunc(1));
            Assert.AreEqual(6, withFalseFunc(6));
        }

        [Test]
        public void ToAction()
        {
            int val = 0;

            Func<int> func = () => { val = 7; return 7; };
            var action = func.ToAction();

            action();
            Assert.AreEqual(7, val);
        }

        [Test]
        public void ToComparer()
        {
            Func<string, string, int> strategy = (x, y) => x.CompareTo(y);
            var comparer = strategy.ToComparer();

            string foo = "Foo";
            string bar = "Bar";

            Assert.AreEqual(foo.CompareTo(bar), comparer.Compare(foo, bar));
        }

        [Test]
        public void ToEqualityComparer_WithSelector()
        {
            Func<string, int> strategy = x => x.Length;
            var lengthComparer = strategy.ToEqualityComparer();

            Assert.IsTrue(lengthComparer.Equals("Foo", "Bar"));
            Assert.AreEqual(lengthComparer.GetHashCode("Foo"), lengthComparer.GetHashCode("Bar"));

            Assert.IsFalse(lengthComparer.Equals("Foo", "Quix"));
            Assert.AreNotEqual(lengthComparer.GetHashCode("Foo"), lengthComparer.GetHashCode("Quix"));
        }

        [Test]
        public void ToEqualityComparer_WithEqualsAndHashCodeStrategy()
        {
            Func<string, string, bool> equalsStrategy = (x, y) => x.Length == y.Length;
            Func<string, int> hashCodeStrategy = x => x.Length.GetHashCode();

            var lengthComparer = equalsStrategy.ToEqualityComparer(hashCodeStrategy);

            Assert.IsTrue(lengthComparer.Equals("Foo", "Bar"));
            Assert.AreEqual(lengthComparer.GetHashCode("Foo"), lengthComparer.GetHashCode("Bar"));

            Assert.IsFalse(lengthComparer.Equals("Foo", "Quix"));
            Assert.AreNotEqual(lengthComparer.GetHashCode("Foo"), lengthComparer.GetHashCode("Quix"));
        }

        [Test]
        public void OrReturningMaybe_WithNullArgument_ReturnsOriginal()
        {
            Func<Maybe<int>> originalFunc = () => 42;
            var func = originalFunc.Or(null);

            var result = func();

            Assert.IsTrue(ReferenceEquals(originalFunc, func));
            Assert.AreEqual(42, result.Value);
        }

        [Test]
        public void OrReturningMaybe_ExtendingNullFunc_ReturnsOriginal()
        {
            Func<Maybe<int>> originalFunc = () => 42;
            var func = ((Func<Maybe<int>>)null).Or(originalFunc);

            var result = func();

            Assert.IsTrue(ReferenceEquals(originalFunc, func));
            Assert.AreEqual(42, result.Value);
        }

        [Test]
        public void OrReturningMaybe_CallsFirstFunc()
        {
            Func<Maybe<int>> left = () => 42;
            Func<Maybe<int>> right = () => 7;

            var func = left.Or(right);

            var results = func();

            Assert.AreEqual(42, results.Value);
        }

        [Test]
        public void OrReturningMaybe_CallsSecondFunc()
        {
            Func<Maybe<int>> left = () => Maybe<int>.NoValue;
            Func<Maybe<int>> right = () => 42;

            var func = left.Or(right);

            var results = func();

            Assert.AreEqual(42, results.Value);
        }

        [Test]
        public void Synchronize_PreventsConcurrentAccess()
        {
            int count = 0;
            Func<int> func = () => { count++; return count; };
            func = func.Synchronize(() => true);

            var random = new Random(DateTime.UtcNow.Second);
            int start = random.Next(10, 30);
            int end = random.Next(50, 100);

            Parallel.For(start, end, x => func());

            Assert.AreEqual(end - start, count);
        }
    }
}
