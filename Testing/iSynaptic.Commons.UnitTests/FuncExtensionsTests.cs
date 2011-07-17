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
