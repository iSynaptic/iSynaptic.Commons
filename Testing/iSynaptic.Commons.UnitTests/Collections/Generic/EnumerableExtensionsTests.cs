using System;
using System.Collections.Generic;
using NUnit.Framework;
using System.Linq;
using System.Collections;
using Rhino.Mocks;

namespace iSynaptic.Commons.Collections.Generic
{
    [TestFixture]
    public class EnumerableExtensionsTests
    {
        [Test]
        public void WithIndex()
        {
            int[] items = { 1, 3, 5, 7, 9 };

            int index = 0;
            foreach (var item in items.WithIndex())
            {
                Assert.AreEqual(index, item.Index);
                Assert.AreEqual(items[index], item.Value);

                index++;
            }
        }

        [Test]
        public void LookAheadEnumerable()
        {
            int[] items = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            using (var enumerator = items.AsLookAheadable().GetEnumerator())
            {
                Assert.IsTrue(enumerator.MoveNext());
                Assert.AreEqual(1, enumerator.Current.Value);
                Assert.AreEqual(2, enumerator.Current.LookAhead(0));

                Assert.IsTrue(enumerator.MoveNext());
                Assert.AreEqual(2, enumerator.Current.Value);
                Assert.AreEqual(3, enumerator.Current.LookAhead(0));
                Assert.AreEqual(5, enumerator.Current.LookAhead(2));
                Assert.AreEqual(4, enumerator.Current.LookAhead(1));

                Assert.IsTrue(enumerator.MoveNext());
                Assert.AreEqual(3, enumerator.Current.Value);
                Assert.AreEqual(4, enumerator.Current.LookAhead(0));
            }

            IEnumerable<int> nullEnumerable = null;
            Assert.Throws<ArgumentNullException>(() => { nullEnumerable.AsLookAheadable(); });
        }

        [Test]
        public void LookAheadEnumeratorPassesOnReset()
        {
            var enumerable = MockRepository.GenerateStub<IEnumerable<int>>();
            var enumerator = MockRepository.GenerateMock<IEnumerator<int>>();

            enumerable.Stub(x => x.GetEnumerator()).Return(enumerator);
            enumerator.Expect(x => x.Reset());

            var la = enumerable.AsLookAheadable();
            var lar = la.GetEnumerator();

            lar.Reset();

            enumerator.VerifyAllExpectations();
        }

        [Test]
        public void LookAheadEnumerableViaNonGenericGetEnumerator()
        {
            int[] items = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var enumerable = items.AsLookAheadable();
            IEnumerator nonGenericEnumerator = null;

            using ((IDisposable)(nonGenericEnumerator = ((IEnumerable)enumerable).GetEnumerator()))
            {
                Assert.IsTrue(nonGenericEnumerator.MoveNext());
                Assert.AreEqual(1, ((LookAheadableValue<int>)nonGenericEnumerator.Current).Value);
                Assert.AreEqual(2, ((LookAheadableValue<int>)nonGenericEnumerator.Current).LookAhead(0));

                Assert.IsTrue(nonGenericEnumerator.MoveNext());
                Assert.AreEqual(2, ((LookAheadableValue<int>)nonGenericEnumerator.Current).Value);
                Assert.AreEqual(3, ((LookAheadableValue<int>)nonGenericEnumerator.Current).LookAhead(0));
                Assert.AreEqual(5, ((LookAheadableValue<int>)nonGenericEnumerator.Current).LookAhead(2));
                Assert.AreEqual(4, ((LookAheadableValue<int>)nonGenericEnumerator.Current).LookAhead(1));

                Assert.IsTrue(nonGenericEnumerator.MoveNext());
                Assert.AreEqual(3, ((LookAheadableValue<int>)nonGenericEnumerator.Current).Value);
                Assert.AreEqual(4, ((LookAheadableValue<int>)nonGenericEnumerator.Current).LookAhead(0));
            }
        }

        [Test]
        public void LookAheadWithNullEnumerator()
        {
            MockRepository mocks = new MockRepository();

            IEnumerable<int> enumerable = mocks.StrictMock<IEnumerable<int>>();
            Expect.Call(enumerable.GetEnumerator()).Return(null);

            mocks.ReplayAll();

            var lookAheadable = enumerable.AsLookAheadable();
            Assert.IsNull(lookAheadable.GetEnumerator());
        }

        [Test]
        public void Buffer()
        {
            bool enumerationComplete = false;
            IEnumerable<int> numbers = GetRange(1, 10, () => { enumerationComplete = true; });

            Assert.IsFalse(enumerationComplete);

            numbers = numbers.Buffer();
            Assert.IsTrue(enumerationComplete);

            Assert.IsTrue(numbers.SequenceEqual(Enumerable.Range(1, 10)));

            IEnumerable<int> nullEnumerable = null;
            Assert.Throws<ArgumentNullException>(() => { nullEnumerable.Buffer(); });
        }

        [Test]
        public void ForceEnumeration()
        {
            bool enumerationComplete = false;
            IEnumerable<int> numbers = GetRange(1, 10, () => { enumerationComplete = true; });

            Assert.IsFalse(enumerationComplete);

            numbers.ForceEnumeration();
            Assert.IsTrue(enumerationComplete);

            IEnumerable<int> nullEnumerable = null;
            Assert.Throws<ArgumentNullException>(() => nullEnumerable.ForceEnumeration());
        }

        [Test]
        public void Delimit()
        {
            IEnumerable<int> range = Enumerable.Range(1, 9);
            Assert.AreEqual("1, 2, 3, 4, 5, 6, 7, 8, 9", range.Delimit(", "));

            IEnumerable<int> nullEnumerable = null;

            Assert.Throws<ArgumentNullException>(() => { nullEnumerable.Delimit(""); });
            Assert.Throws<ArgumentNullException>(() => { range.Delimit(null); });
            Assert.Throws<ArgumentNullException>(() => { range.Delimit("", null); });
        }

        [Test]
        public void Zip_WithNullIterables_ThrowsArgumentNullException()
        {
            IEnumerable<int> first = null;
            IEnumerable<int> other = Enumerable.Range(1, 10);

            Assert.Throws<ArgumentNullException>(() => first.Zip(other));
            Assert.Throws<ArgumentNullException>(() => other.Zip(null));
        }

        [Test]
        public void Zip()
        {
            var rangeOne = Enumerable.Range(1, 10);
            var rangeTwo = Enumerable.Range(10, 10);

            var zipped = rangeOne.Zip(rangeTwo);

            var expected = new[] {1, 10, 2, 11, 3, 12, 4, 13, 5, 14, 6, 15, 7, 16, 8, 17, 9, 18, 10, 19};

            Assert.IsTrue(zipped.SelectMany(x => x).SequenceEqual(expected));
        }

        [Test]
        public void Zip_ArrayOfEnumerables()
        {
            var array = new[] {Enumerable.Range(1, 10), Enumerable.Range(10, 10)};
            var zipped = array.Zip();

            var expected = new[] { 1, 10, 2, 11, 3, 12, 4, 13, 5, 14, 6, 15, 7, 16, 8, 17, 9, 18, 10, 19 };

            Assert.IsTrue(zipped.SelectMany(x => x).SequenceEqual(expected));
        }

        [Test]
        public void Zip_EnumerableOfEnumerables()
        {
            var array = new[] { Enumerable.Range(1, 10), Enumerable.Range(10, 10) };
            var zipped = array.AsEnumerable().Zip();

            var expected = new[] { 1, 10, 2, 11, 3, 12, 4, 13, 5, 14, 6, 15, 7, 16, 8, 17, 9, 18, 10, 19 };

            Assert.IsTrue(zipped.SelectMany(x => x).SequenceEqual(expected));
        }

        [Test]
        public void AllSatisfy()
        {
            Func<int, bool> isEven = x => x%2 == 0;
            var spec = isEven.ToSpecification();

            var numbers = new[] {2, 4, 6, 8, 10};
            Assert.IsTrue(numbers.AllSatisfy(spec));
        }

        [Test]
        public void ToDictionary_WithNull_ThrowsArgumentNullException()
        {
            IEnumerable<KeyValuePair<string, string>> pairs = null;
            Assert.Throws<ArgumentNullException>(() => pairs.ToDictionary());
        }

        private static IEnumerable<int> GetRange(int start, int end, Action after)
        {
            foreach (int i in Enumerable.Range(start, end))
                yield return i;

            if (after != null)
                after();
        }
    }
}
